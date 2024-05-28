using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Interop;

using GitLabWpfApp.Components;
using GitLabWpfApp.Views;

namespace GitLabWpfApp.Processing;

internal class ProcessStarter
{
    //\\HOSTNAME\c$
    //c:

    private static readonly string _idleBusterExecutableFile = @"C:\IdleBuster.exe";

    private static readonly string _yamlFile =
        @"c:\my.yaml";

    private static readonly string? _yamlFolder = Path.GetDirectoryName(_yamlFile);
    private readonly ISolutionTracker st  ;

    public ProcessStarter(ISolutionTracker st)
    {
        this.st = st;
    }

    public static void StarEditYaml(EditWith editWith)
    {
   
        var processStartInfo = new ProcessStartInfo
        {
            FileName = @"C:\Program Files\Notepad++\notepad++.exe",
            //WorkingDirectory = @"c:\work",
            ArgumentList =
            {
                _yamlFile
            }
        };

        Process.Start(processStartInfo);
    }

    public static void GitCommitYaml()
    {
        var path = _yamlFolder;
        var msg = @"[DWS-] updated images";
        GitCommitProject(path, msg);
    }



    public static void OpenSolutionForJira(ISolutionTracker _solutionTracker, string jira)
    {
        if (!string.IsNullOrWhiteSpace(jira))
        {
            var folder = $@"c:\Work\solutionfolder";
            //EnsureFolderExists();
            var sln = Path.Combine(folder, $@"{jira}.sln");
            if (!File.Exists(sln))
            {
                MessageBox.Show("no solution yet.");
                return;
            }
        
                if (_solutionTracker.HasSolution(sln))
                    _solutionTracker.BringUp(sln);
                else
                    _solutionTracker.Start(sln);
        }
    }

    private static void EnsureFolderExists(string folder)
    {
        var directoryName = Path.GetDirectoryName(folder);
        if (directoryName == "c:\\")
            return;
        EnsureFolderExists(directoryName);
        var path = Path.GetFileName(folder);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
    }



    public static void StartAppIdleBuster()
    {
        ProcessStartHelper.OpenApp(_idleBusterExecutableFile);
    }

    public static void StartAppCherry()
    {
        ProcessStartHelper.OpenApp(@"C:\cherrytree.exe");
    }

    public static void StartAppKeyPass()
    {
        ProcessStartHelper.OpenApp(@"C:\KeePass.exe");
    }

    public static void StartAlwwaysOnTop()
    {
        ProcessStartHelper.OpenApp(@"C:\AlwaysOnTopMaker.exe");
    }
    public static void StartScreenshots()
    {
        ProcessStartHelper.OpenApp(@"C:\ScreenGrabber.exe");
    }
    public static void GitCommitProject(string? path, string msg)
    {
        ProcessStartHelper.OpenApp(@"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe",
            $@"/command:commit /path ""{path}"" /logmsg ""{msg}""");
    }
    public static void GitShowLogForProject(string path)
    {
        ProcessStartHelper.OpenApp(@"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe",
            $@"/command:log /path ""{path}""");
    }

    public static void OpenFork()
    {
        ProcessStartHelper.OpenApp(@"C:\Fork.exe");
    }

    public static void OpenJira(string jiraDataCurrentTicketName)
    {
        ProcessStartHelper.OpenUrl($"https://My.atlassian.net/browse/{jiraDataCurrentTicketName}");
    }
}