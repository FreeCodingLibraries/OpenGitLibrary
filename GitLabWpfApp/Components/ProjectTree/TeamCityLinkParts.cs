namespace GitLabWpfApp.Components.ProjectTree;

public class TeamCityLinkParts
{
    public TeamCityLinkParts(string mainPart, string buildFolder)
    {
        MainPart = mainPart; //project
        BuildFolder = buildFolder; //buildConfiguration
    }

    public string MainPart { get; set; }
    public string BuildFolder { get; set; }
}