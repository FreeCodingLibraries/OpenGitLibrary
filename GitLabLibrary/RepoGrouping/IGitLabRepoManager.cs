namespace GitLabLibrary.RepoGrouping;

public interface IGitLabRepoManager
{
    List<GitLabProjectContext> FetchGitLabProjects();
}