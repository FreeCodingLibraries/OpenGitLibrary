using System.Runtime.InteropServices;

namespace GitLabWpfApp.Processing;

internal class ForeGroundSetter
{
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(nint hWnd);
}