using GitLabLibrary.Git.Results;

namespace GitLabLibrary.Services.GroupRepoUpdater.Events;

public class RepoClonedEvent : RepoChangedEvent
{
    public GitCloneResult Result { get; set; }
}