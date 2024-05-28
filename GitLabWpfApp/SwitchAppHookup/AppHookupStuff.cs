using System.Runtime.InteropServices;
using System.Windows;
using GitLabWpfApp.Framework.Logging;
using static GitLabWpfApp.SwitchAppHookup.DllFuncs;

namespace GitLabWpfApp.SwitchAppHookup;

public class AppHookupStuff : IDisposable, IAppHookupStuff
{
    private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
    private const uint WINEVENT_OUTOFCONTEXT = 0;

    private readonly uint _windowThreadId;
    private readonly ILogger _logger;

    private IntPtr _previouslyActiveWindow = IntPtr.Zero;

    private WinEventDelegate _winEventDelegate;

    private IntPtr _winEventHook = IntPtr.Zero;

    public AppHookupStuff(uint windowThreadId, ILogger logger)
    {
        _windowThreadId = windowThreadId;
        _logger = logger;
    }

    public AppHookupStuff Setup()
    {
        // Set up the WinEvent hook
        _winEventDelegate = WinEventProc;
        _winEventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero,
            _winEventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);

        if (_winEventHook == IntPtr.Zero)
        {
            var errorCode = Marshal.GetLastWin32Error();
            MessageBox.Show("Failed to set WinEvent hook, error code: " + errorCode);
        }

        return this;
    }

    public void SwitchToLastActiveApp()
    {
        RestorePreviousWindow();
    }


    public void Dispose()
    {
        if (_winEventHook != IntPtr.Zero)
            UnhookWinEvent(_winEventHook);
    }

    private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
        uint dwEventThread, uint dwmsEventTime)
    {
        // Store the previously active window before the new window is activated
        var isPartOfApp = IsWindowPartOfCurrentApp(hwnd);
        if (hwnd != IntPtr.Zero && !isPartOfApp)
        {

            _previouslyActiveWindow = hwnd;
            string windowTitle = DllFuncs.GetWindowTitle(hwnd);
            string classname = DllFuncs.GetWindowClassName(hwnd);
            _logger.Log($"-------------" +
                        $"\nClass: {classname} ({hwnd})" +
                        $"\nTitle: {windowTitle} ");
        }
    }

    private void RestorePreviousWindow()
    {
        if (_previouslyActiveWindow != IntPtr.Zero)
        {
            var targetThreadId = GetWindowThreadProcessId(_previouslyActiveWindow, out _);
            var currentThreadId = GetCurrentThreadId();

            if (targetThreadId != currentThreadId)
            {
                AttachThreadInput(currentThreadId, targetThreadId, true);
                SetForegroundWindow(_previouslyActiveWindow);
                AttachThreadInput(currentThreadId, targetThreadId, false);
            }
            else
            {
                SetForegroundWindow(_previouslyActiveWindow);
            }
        }
    }


    private bool IsWindowPartOfCurrentApp(IntPtr windowHandle)
    {
        // Get the thread ID of the window handle
        var windowThreadId = GetWindowThreadProcessId(windowHandle, out _);

        // Compare the thread IDs
        return _windowThreadId == windowThreadId;
    }
}