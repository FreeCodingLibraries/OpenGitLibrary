namespace GitLabLibrary.SolutionMaker.Basics;

public class Solution
{
    public string Guid { get; set; }
    public List<Project> Projects { get; set; } = new();
    public List<SolutionFolder> SolutionFolders { get; set; } = new();

    public void Add(Project project)
    {
        Projects.Add(project);
    }
}