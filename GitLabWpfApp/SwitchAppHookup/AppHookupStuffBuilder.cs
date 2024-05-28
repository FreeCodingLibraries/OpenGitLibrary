using System.Windows;
using System.Windows.Interop;
using GitLabWpfApp.Framework.Logging;

namespace GitLabWpfApp.SwitchAppHookup;

public class AppHookupStuffBuilder : IAppHookupStuffBuilder
{
    private readonly ILogger _logger;
    private uint _windowThreadId;

    public AppHookupStuffBuilder(ILogger logger)
    {
        _logger = logger;
    }

    public AppHookupStuffBuilder ExcludeThreadByWindow(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;
        var windowThreadId = DllFuncs.GetWindowThreadProcessId(windowHandle, out _);
        _windowThreadId = windowThreadId;
        return this;
    }

    public AppHookupStuff Build() => new(_windowThreadId, _logger);
}