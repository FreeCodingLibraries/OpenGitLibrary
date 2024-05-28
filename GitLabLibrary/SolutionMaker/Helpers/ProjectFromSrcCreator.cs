using GitLabLibrary.SolutionMaker.Basics;

namespace GitLabLibrary.SolutionMaker.Helpers;

public static class ProjectFromSrcCreator
{
    private static string MakeProjectName(string srcPath)
    {
        //not that this matters
        var directoryName = Path.GetFileNameWithoutExtension(srcPath);
        return directoryName;
    }

    public static Project ProjFromSrc(string srcPath, SolutionFolder parent)
    {
        return new Project()
        {
            Parent = parent,
            ProjName = MakeProjectName(srcPath),
            SrcProjAbsolutePath = srcPath,
            ProjGuid = Guid.NewGuid().ToString(),
            ProjectTypeGuid = ProjectTypes.CSharp_SDK_Project_System
        };
    }
}