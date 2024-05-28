using System.Text;

using GitLabLibrary.SolutionMaker.Helpers;

namespace GitLabLibrary.SolutionMaker.Basics;

public class SolutionWriter
{
    public string GenerateCode(Solution solution, string solutionPath)
    {
        var sb = new StringBuilder();
        sb.Append(@$"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.8.34330.188
MinimumVisualStudioVersion = 10.0.40219.1
");
        foreach (var p in solution.Projects)
        {
            var relativeProjectPath = PathHelper.PathDiff(solutionPath, p.SrcProjAbsolutePath);
            sb.Append(
                $@"Project(""{{{ProjectTypes.CSharp_SDK_Project_System}}}"") = ""{p.ProjName}"", ""{relativeProjectPath}"", ""{{{p.ProjGuid}}}""
EndProject
");
        }

        foreach (var p in solution.SolutionFolders)
        {
            sb.Append(
                $@"Project(""{{{ProjectTypes.Solution_Folder}}}"") = ""{p.FolderName}"", ""{p.FolderName}"", ""{{{p.FolderGuid}}}""
EndProject
");

            //   Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "NewFolder1", "NewFolder1", "{BA28F358-5906-4715-8916-606B427C231F}"
            //   EndProject
        }

        sb.Append(@$"Global
");

        if (solution.Projects.Any())
            sb.Append($@"	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
");

        foreach (var p in solution.Projects)
        {
            string TmpPart2 = $@"		{{{p.ProjGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{{p.ProjGuid}}}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{{{p.ProjGuid}}}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{{{p.ProjGuid}}}.Release|Any CPU.Build.0 = Release|Any CPU
";
            sb.Append(TmpPart2);
        }


        if (solution.Projects.Any())
            sb.Append(@$"	EndGlobalSection
");


        // project / solution folder - hierachy
        var hasHierachy = false;
        var subSb = new StringBuilder();
        foreach (var p in solution.Projects)
        {
            if (p.Parent != null)
            {
                subSb.Append(@$"		{{{p.ProjGuid}}} = {{{p.Parent.FolderGuid}}}
");
                hasHierachy = true;
            }
        }

        foreach (var p in solution.SolutionFolders)
        {
            if (p.Parent != null)
            {
                subSb.Append(@$"		{{{p.FolderGuid}}} = {{{p.Parent.FolderGuid}}}
");
                hasHierachy = true;
            }
        }

        sb.Append(@$"	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
");
        if (hasHierachy)
            sb.Append(@$"	GlobalSection(NestedProjects) = preSolution
{subSb.ToString()}	EndGlobalSection
");


        sb.Append(@$"	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {{{solution.Guid}}}
	EndGlobalSection
EndGlobal
");
        return sb.ToString();
    }
}