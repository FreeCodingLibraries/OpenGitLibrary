using System.IO;
using GitLabLibrary.Git;
using GitLabLibrary.RepoGrouping;
using GitLabLibrary.Services.GroupRepoUpdater;

namespace GitLabSyncConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        //DEV CODE:
        //  new GitLabProjectsManager().FetchGitLabProjects();

        var startTime = DateTime.Now;
        var localRepoPath = GetNormalisedPath(@"C:\Work\code\");
        //var localRepoPath = GetNormalisedPath(CurrentAppConfig.CodeFolder ); 
        Console.WriteLine($"started at: {DateTime.Now.ToLongTimeString()}");
        Console.WriteLine($"local path: {localRepoPath}");

        var gitLabRepoManager = new HardcodedGitLabProjectsManager();
        var gitClient = new Libgit2sharpClient();

        var groupCloneOptions = new GroupCloneOptions
        {
            SkipRepos = new List<string>(), // { "root/devops/build-tools/teamcity" },
            OverrideSettings = new List<OverrideRepoSettings>
            {
                new() { RepoPath = "root/proj/clients/name-contracts", DefaultBranch = "master" }
            }
        };

        var repoGroupCloneUpdater = new RepoGroupCloneUpdateBuilder(gitLabRepoManager, gitClient)
            .WithOptions(groupCloneOptions)
            .Build();

        var _logs = "";

        repoGroupCloneUpdater.NewRepoWasCloned += (sender, e) =>
        {
            Console.WriteLine("*** NEW REPO ***");
            Console.WriteLine($"    extract {e.GitLabRepoPathWithNamespace}");
            Console.WriteLine(
                $"    branch {e.DefaultRemoteBranch} {(e.DefaultRemoteBranch != "master" ? " (not master!)" : "")}");
            Console.WriteLine($"    to {e.LocalRepoPath}");

            if (!e.Result.Success)
                _logs += $"can't clone '{e.RemoteRepoHttpUrl}'\n";

            Console.WriteLine($"    {(e.Result.Success ? "success" : "FAILED!")}");
        };
        repoGroupCloneUpdater.RepoSynced += (sender, e) =>
        {
            Console.WriteLine($"..SYNC REPO ... {e.GitLabRepoPathWithNamespace}");
            Console.WriteLine($"    under: {e.LocalRepoPath}");
            if (!e.Result.RequireMerge)
                Console.WriteLine($"        local branch '{e.DefaultRemoteBranch}' is already in sync with remote");
            if (!e.Result.Success)
                _logs += $"can't pull '{e.LocalRepoPath}' branch '{e.DefaultRemoteBranch}'\n";
            Console.WriteLine($"    {(e.Result.Success ? "success" : "FAILED!")}");
        };

        repoGroupCloneUpdater.CloneOrUpdateAllRepos();

        var endTime = DateTime.Now;

        var durationSecTotal = endTime.Subtract(startTime).TotalSeconds;
        var durationHr = endTime.Subtract(startTime).Hours;
        var durationMin = endTime.Subtract(startTime).Minutes;
        var durationSec = endTime.Subtract(startTime).Seconds;


        Console.WriteLine();
        Console.WriteLine(_logs);
        Console.WriteLine();
        Console.WriteLine($"Duration: {durationHr}hr {durationMin}min {durationSec}sec");
        Console.WriteLine("---------------------\ndone");

         }

    private static string GetNormalisedPath(string path)
    {
        return Path.GetFullPath(new Uri(path).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        //.ToUpperInvariant();
    }
}