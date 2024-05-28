namespace GitLabLibrary.SolutionMaker.Basics;

public class Project
{
    public SolutionFolder Parent { get; set; }
    public string TmpContent { get; set; }
    public string ProjGuid { get; set; }
    public string ProjName { get; set; }
    public string ProjectTypeGuid { get; set; }
    public string SrcProjAbsolutePath { get; set; }
}