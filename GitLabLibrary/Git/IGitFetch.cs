namespace GitLabLibrary.Git;

public interface IGitFetch
{
    bool Fetch(string targetPath, string origin = "origin");
}