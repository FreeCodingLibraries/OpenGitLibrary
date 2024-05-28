using System.Diagnostics;

namespace GitLabWpfApp.Processing;

internal class ProcessStartHelper
{
    public static void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url)
        {
            UseShellExecute = true
        });
    }

    public static void OpenRun(string path, string args = null)
    {
        Process.Start(new ProcessStartInfo(path, args)
        {
            UseShellExecute = true
        });
    }

    public static void OpenFolder(string path)
    {
        Process.Start(new ProcessStartInfo(path)
        {
            UseShellExecute = true
        });
    }

    public static void OpenApp(string exec, string args = null)
    {
        Process.Start(exec, args);
    }
}