namespace GitLabLibrary.Services.GroupRepoUpdater;

public class GroupCloneOptions
{
    public List<string> SkipRepos { get; set; } //use path with namespaces
    public List<OverrideRepoSettings> OverrideSettings { get; set; }

    public OverrideRepoSettings OverrideSettingsForRepoPath(string repositoryContextPathWithNamespace)
    {
        var overrideSettingsForRepoPath =
            OverrideSettings.FirstOrDefault(m => m.RepoPath == repositoryContextPathWithNamespace);
        if (overrideSettingsForRepoPath != null)
            ;
        return overrideSettingsForRepoPath;
    }
}