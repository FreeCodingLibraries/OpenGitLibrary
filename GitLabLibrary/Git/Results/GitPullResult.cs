namespace GitLabLibrary.Git.Results;

public class GitPullResult
{
    public GitPullResult(bool Success, bool requireMerge, MyMergeStatus? mergeStatus = null, string failure = null)
    {
        this.Success = Success;
        RequireMerge = requireMerge;
        MergeStatus = mergeStatus;
        Failure = failure;
    }

    public bool Success { get; }
    public bool RequireMerge { get; }
    public MyMergeStatus? MergeStatus { get; }
    public string Failure { get; }
}