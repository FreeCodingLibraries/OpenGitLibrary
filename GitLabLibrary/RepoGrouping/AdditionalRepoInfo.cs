namespace GitLabLibrary.RepoGrouping;

public class AdditionalRepoInfo
{
    public AdditionalRepoInfo(GitLabProjectContext gitLabProjectContext)
    {
        GitLabProjectContext = gitLabProjectContext;
    }

    public GitLabProjectContext GitLabProjectContext { get; set; }
    public bool FolderExists { get; set; }
    public string LocalRepoPath { get; set; }
}