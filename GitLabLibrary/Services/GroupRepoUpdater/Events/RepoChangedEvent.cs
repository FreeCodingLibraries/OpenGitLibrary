namespace GitLabLibrary.Services.GroupRepoUpdater.Events;

public class RepoChangedEvent
{
    public string RemoteRepoHttpUrl { get; set; }
    public string GitLabRepoPathWithNamespace { get; set; }
    public string LocalRepoPath { get; set; }
    public string DefaultRemoteBranch { get; set; }
}