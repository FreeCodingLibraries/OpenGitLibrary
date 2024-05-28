using System.Runtime.InteropServices;
using System.Text;

namespace GitLabWpfApp.SwitchAppHookup;

public static class DllFuncs
{
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
        uint dwEventThread, uint dwmsEventTime);

    // Import user32.dll functions
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    [DllImport("user32.dll")]
    public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc,
        WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    [DllImport("user32.dll")]
    public static extern bool UnhookWinEvent(IntPtr hWinEventHook);



    /* Window (e.g. text)
     */

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool IsWindow(IntPtr hWnd);


    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    public static string GetWindowTitle(IntPtr hWnd)
    {
        if (!IsWindow(hWnd))
        {
            throw new InvalidOperationException("Invalid window handle.");
        }

        int length = GetWindowTextLength(hWnd);
        if (length == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder(length + 1);
        GetWindowText(hWnd, sb, sb.Capacity);

        return sb.ToString();
    }
    public static string GetWindowClassName(IntPtr hWnd)
    {
        if (!IsWindow(hWnd))
        {
            throw new InvalidOperationException("Invalid window handle.");
        }

        StringBuilder sb = new StringBuilder(500);
        //sb.Clear();
        GetClassName(hWnd, sb, sb.Capacity);

        return sb.ToString();
    }


    /*find windows*/
    // Import user32.dll functions
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


    public static IntPtr FindWindowByClassNameAndTitle(string className, string windowTitle)
    {
        return FindWindow(className, windowTitle);
    }

    public static bool SetFocusToWindow(IntPtr hWnd)
    {
        if (hWnd != IntPtr.Zero)
        {
            return SetForegroundWindow(hWnd);
        }
        return false;
    }
}