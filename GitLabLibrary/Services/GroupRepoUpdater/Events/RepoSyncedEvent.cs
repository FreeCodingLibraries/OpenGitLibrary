using GitLabLibrary.Git.Results;

namespace GitLabLibrary.Services.GroupRepoUpdater.Events;

public class RepoSyncedEvent : RepoChangedEvent
{
    public GitPullResult Result { get; set; }
}