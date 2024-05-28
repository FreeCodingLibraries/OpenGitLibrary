using System.Diagnostics;

namespace GitLabWpfApp.Processing;

internal static class RunningProcessHelper
{
    public static bool IsIdleBusterRunning => IsProcessByNameRunning("IdleBuster");
    public static bool IsCherryRunning => IsProcessByNameRunning("cherrytree");
    public static bool IsKeyPassRunning => IsProcessByNameRunning("KeePass");
    public static bool IsAotRunning => IsProcessByNameRunning("AlwaysOnTopMaker");
    public static bool IsScreenshotsRunning => IsProcessByNameRunning("ScreenGrabber");

    private static bool IsProcessByNameRunning(string procName)
    {
        return Process.GetProcessesByName(procName).Length > 0;
    }

    public static bool GetDevEnvHandleForSolution(string sln)
    {
        var processesByName = Process.GetProcessesByName("devenv");
        ;
        return false;
    }
}