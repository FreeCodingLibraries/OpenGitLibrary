using System.Diagnostics;
using GitLabLibrary.Git.Results;

namespace GitLabLibrary.Git;

public class ConsoleGitClient : IGitClone, IGitPull
{
    //https://stackoverflow.com/questions/1911109/how-do-i-clone-a-specific-git-branch

    public GitCloneResult Clone(string repoUrl, string targetPath, string branch, string origin = "origin")
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone -c http.sslVerify=false --single-branch -b {branch} {repoUrl} {targetPath}",
                RedirectStandardError = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
            return new GitCloneResult(false);

        return new GitCloneResult(true);
    }

    public GitPullResult Pull(string targetPath, string branch, string origin = "origin")
    {
        throw new Exception("not fully implemented, to pull a given branch");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = targetPath,
                FileName = "git",
                Arguments = "pull",
                RedirectStandardError = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                CreateNoWindow = true
            }
        };
        //string output = p.StandardOutput.ReadToEnd();
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0) return new GitPullResult(false, true);

        return new GitPullResult(true, true);
    }
}