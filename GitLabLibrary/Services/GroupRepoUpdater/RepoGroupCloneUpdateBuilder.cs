using GitLabLibrary.Git;
using GitLabLibrary.RepoGrouping;

namespace GitLabLibrary.Services.GroupRepoUpdater;

public class RepoGroupCloneUpdateBuilder
{
    private readonly IGitClient _gitClient;
    private readonly HardcodedGitLabProjectsManager _gitLabProjectsManager;
    private GroupCloneOptions _groupCloneOptions;

    public RepoGroupCloneUpdateBuilder(HardcodedGitLabProjectsManager gitLabProjectsManager, IGitClient gitClient)
    {
        _gitLabProjectsManager = gitLabProjectsManager;
        _gitClient = gitClient;
    }

    public RepoGroupCloneUpdater Build()
    {
        return new RepoGroupCloneUpdater(_gitLabProjectsManager, _gitClient, _groupCloneOptions);
    }

    public RepoGroupCloneUpdateBuilder WithOptions(GroupCloneOptions groupCloneOptions)
    {
        _groupCloneOptions = groupCloneOptions;
        return this;
    }
}