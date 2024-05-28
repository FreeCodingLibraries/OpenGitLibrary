using GitLabLibrary.Git;
using GitLabLibrary.RepoGrouping;
using GitLabLibrary.Services.GroupRepoUpdater.Events;

namespace GitLabLibrary.Services.GroupRepoUpdater;

public class RepoGroupCloneUpdater
{
    private readonly IGitClient _gitClient;
    private readonly HardcodedGitLabProjectsManager _gitLabProjectsManager;
    private readonly GroupCloneOptions _groupCloneOptions;

    public RepoGroupCloneUpdater(HardcodedGitLabProjectsManager gitLabProjectsManager, IGitClient gitClient,
        GroupCloneOptions groupCloneOptions)
    {
        _gitLabProjectsManager = gitLabProjectsManager;
        _gitClient = gitClient;
        _groupCloneOptions = groupCloneOptions;
    }


    public void CloneOrUpdateAllRepos()
    {
        var logs = "";
        var repositoryContexts = _gitLabProjectsManager
            .FetchGitLabProjects()
            //.Where(repoC => repoC.http_url_to_repo.Contains("aws", StringComparison.InvariantCultureIgnoreCase))
            .Where(repoC => !_groupCloneOptions.SkipRepos.Contains(repoC.path_with_namespace))
            .ToArray();

        var augmentedRepositoryContexts = repositoryContexts;

        var repoCount = repositoryContexts.Count();

        var i = 1;
        foreach (var repo in augmentedRepositoryContexts)
        {
            /*var overrideForRepo =
                _groupCloneOptions.OverrideSettingsForRepoPath(repo.GitLabProjectContext.path_with_namespace);
            var thisRepoDefaultBranch = repo.GitLabProjectContext.default_branch; //?????
            var defaultRemoteBranch = overrideForRepo?.DefaultBranch ?? "master";

            Console.WriteLine(" ");

            Console.WriteLine($".. {i} / {repoCount} = {i * 100 / repoCount} %");
            if (repo.FolderExists)
            {
                var result = _gitClient.Pull(repo.LocalRepoPath, defaultRemoteBranch);

                OnRepoSynced(new RepoSyncedEvent
                {
                    RemoteRepoHttpUrl = repo.GitLabProjectContext.http_url_to_repo,
                    GitLabRepoPathWithNamespace = repo.GitLabProjectContext.path_with_namespace,
                    LocalRepoPath = repo.LocalRepoPath,
                    DefaultRemoteBranch = defaultRemoteBranch,
                    Result = result
                });
            }
            else
            {
                var result = _gitClient.Clone(repo.GitLabProjectContext.http_url_to_repo, repo.LocalRepoPath,
                    defaultRemoteBranch);

                //              continue; //todo: remove

                OnNewRepoWasCloned(new RepoClonedEvent
                {
                    RemoteRepoHttpUrl = repo.GitLabProjectContext.http_url_to_repo,
                    GitLabRepoPathWithNamespace = repo.GitLabProjectContext.path_with_namespace,
                    LocalRepoPath = repo.LocalRepoPath,
                    DefaultRemoteBranch = defaultRemoteBranch,
                    Result = result
                });
            }

            i++;*/
        }

        Console.WriteLine("");
        Console.WriteLine($"done at: {DateTime.Now.ToLongTimeString()}");
        Console.WriteLine("");
        Console.WriteLine("Log:");
        Console.WriteLine(logs);
        Console.WriteLine("");
    }

    #region events

    public event EventHandler<RepoClonedEvent> NewRepoWasCloned;

    public void OnNewRepoWasCloned(RepoClonedEvent e)
    {
        NewRepoWasCloned?.Invoke(this, e);
    }

    public event EventHandler<RepoSyncedEvent> RepoSynced;

    public void OnRepoSynced(RepoSyncedEvent e)
    {
        RepoSynced?.Invoke(this, e);
    }

    #endregion
}