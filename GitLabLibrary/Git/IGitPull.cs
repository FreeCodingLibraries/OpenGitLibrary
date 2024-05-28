using GitLabLibrary.Git.Results;

namespace GitLabLibrary.Git;

public interface IGitPull
{
    GitPullResult Pull(string targetPath, string branch, string origin = "origin");
}