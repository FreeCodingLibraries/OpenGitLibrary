namespace GitLabLibrary.Git.Results;

public class GitCloneResult
{
    public GitCloneResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; set; }
}