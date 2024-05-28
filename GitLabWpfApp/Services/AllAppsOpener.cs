using GitLabWpfApp.Processing;

namespace GitLabWpfApp.Services;

public class AllAppsOpener
{
    public static void OpenAll()
    {
        if (!RunningProcessHelper.IsIdleBusterRunning)
            ProcessStarter.StartAppIdleBuster();

        if (!RunningProcessHelper.IsCherryRunning)
            ProcessStarter.StartAppCherry();

        if (!RunningProcessHelper.IsKeyPassRunning)
            ProcessStarter.StartAppKeyPass();

        if (!RunningProcessHelper.IsAotRunning)
            ProcessStarter.StartAlwwaysOnTop();

        if (!RunningProcessHelper.IsScreenshotsRunning)
            ProcessStarter.StartScreenshots();

    }
}