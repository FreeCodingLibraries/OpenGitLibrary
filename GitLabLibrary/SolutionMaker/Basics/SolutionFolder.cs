namespace GitLabLibrary.SolutionMaker.Basics;

public class SolutionFolder
{
    public SolutionFolder Parent { get; set; }
    public string FolderName { get; }
    public Guid FolderGuid { get; set; }

    public SolutionFolder(string folderName, SolutionFolder parent = null, Guid id = default)
    {
        Parent = parent;
        FolderName = folderName;
        FolderGuid = id != default ? id : Guid.NewGuid();
    }
}