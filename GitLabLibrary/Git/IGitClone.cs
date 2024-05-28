using GitLabLibrary.Git.Results;

namespace GitLabLibrary.Git;

public interface IGitClone
{
    GitCloneResult Clone(string repoUrl, string targetPath, string branch, string origin = "origin");
}