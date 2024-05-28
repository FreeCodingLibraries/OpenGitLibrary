using System.Text.Json;

using GitLabLibrary._UserSpecific;

namespace GitLabLibrary.RepoGrouping;

public class HardcodedGitLabProjectsManager : IGitLabRepoManager
{
    public List<GitLabProjectContext> FetchGitLabProjects()
    {
        var serialisedData = File.ReadAllText(Settings.FetchGitLabProjectsFolder);
        var repositoryContexts = JsonSerializer.Deserialize<List<GitLabProjectContext>>(serialisedData);
        return repositoryContexts;
    }
}