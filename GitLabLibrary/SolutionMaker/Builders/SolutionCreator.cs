using GitLabLibrary.SolutionMaker.Basics;
using GitLabLibrary.SolutionMaker.Helpers;

namespace GitLabLibrary.SolutionMaker.Builders;

public class SolutionCreator
{
    private List<Project> _projects = new();
    private List<SolutionFolder> _folders = new();

    public SolutionCreator AddProjectFrom(string srcPath, SolutionFolder parent = null)
    {
        var projFromSrc = ProjectFromSrcCreator.ProjFromSrc(srcPath, parent);
        var nm = projFromSrc.ProjName;
        int i = 2;
        while (_projects.Any(p => p.ProjName == projFromSrc.ProjName))
        {
            projFromSrc = new Project()
            {
                ProjName = nm+(i++).ToString(),
                Parent = projFromSrc.Parent,
                ProjGuid = projFromSrc.ProjGuid,
                SrcProjAbsolutePath = projFromSrc.SrcProjAbsolutePath,
                ProjectTypeGuid = projFromSrc.ProjectTypeGuid,
                TmpContent = projFromSrc.TmpContent
            };
        }

        _projects.Add(projFromSrc);
        return this;
    }

    public void WriteTo(string saveLocation)
    {
        var dirOnly = Path.GetDirectoryName(saveLocation);
        var solution = new Solution
        {
            Guid = Guid.NewGuid().ToString(),
            Projects = _projects,
            SolutionFolders = _folders
        };

        var generateCode = new SolutionWriter().GenerateCode(solution, dirOnly);
        File.WriteAllText(saveLocation, generateCode);
    }
    public string AsText(string saveLocation)
    {
        var dirOnly = Path.GetDirectoryName(saveLocation);
        var solution = new Solution
        {
            Guid = Guid.NewGuid().ToString(),
            Projects = _projects,
            SolutionFolders = _folders
        };

        var generateCode = new SolutionWriter().GenerateCode(solution, dirOnly);
        return generateCode;
    }

    public SolutionFolder AddSolutionFolder(string folderName, SolutionFolder parent = null, Guid id = default)
    {
        var solutionFolder = new SolutionFolder(folderName, parent, id);
        _folders.Add(solutionFolder);
        return solutionFolder;
    }

    public SolutionCreator AddProjectsUnder(string srcFolder, int groupByDepth, SolutionFolder? mainPrentFolder, Predicate<string> filepathcondition = null)
    {
        var sg = new SolutionFolderGenerator();
        Dictionary<string, SolutionFolder> parentFolders = new Dictionary<string, SolutionFolder>();
        var allProjectFiles = RecursiveProjectFinder.GetAllRecursiveProjectFiles(srcFolder);
        foreach (var projectFile in allProjectFiles)
        {
            var folderPathOnly = Path.GetDirectoryName(projectFile);
            var fileNameOnly = Path.GetFileName(projectFile);

            var partialFolderPath = folderPathOnly.Substring( srcFolder.Length+1);
            var subs = partialFolderPath.Split(Path.DirectorySeparatorChar);
            var usedSubs = subs.Take(subs.Length > 1 ? subs.Length-1 : 0);
            if (filepathcondition == null || filepathcondition(projectFile))
            {
                SolutionFolder parentFolder = sg.Get(usedSubs, this);
                AddProjectFrom(projectFile, parentFolder);
            }
        }

        return this;
    }
}

public class SolutionFolderGenerator
{
    private Dictionary<string, SolutionFolder> folders = new();
    public SolutionFolder Get(IEnumerable<string> caseSensitiveUsedSubs, SolutionCreator sc)
    {
        IEnumerable<string> usedSubs = caseSensitiveUsedSubs.Select(s=>s.ToLower());
        var strSoFar = "";
        SolutionFolder parentSoFar = null;
        foreach (var sub in usedSubs)
        {
            strSoFar = Path.Join(strSoFar, sub);
            SolutionFolder solutionFolder;
            if (!folders.ContainsKey(strSoFar))
            {
                //solutionFolder = new SolutionFolder(sub,parentSoFar);
                solutionFolder = sc.AddSolutionFolder(sub, parentSoFar);
                folders.Add(strSoFar,solutionFolder);
            }
            else
            {
                solutionFolder = folders[strSoFar];
            }
                parentSoFar = solutionFolder;
        }

        return parentSoFar;
    }

    public IEnumerable<SolutionFolder> Folders { get; set; }
}

public class RecursiveProjectFinder
{
    public static IEnumerable<string> GetAllRecursiveProjectFiles(string srcFolder)
    {
        List<string> csprojFiles = new();
        var excludedFolders = new HashSet<string> { "obj", "bin" };

        // Get all .csproj files in the current directory
        csprojFiles.AddRange(Directory.GetFiles(srcFolder, "*.csproj", SearchOption.TopDirectoryOnly));

        // Recurse into subdirectories
        foreach (var directory in Directory.GetDirectories(srcFolder))
        {
            var dirName = new DirectoryInfo(directory).Name;
            if (!excludedFolders.Contains(dirName.ToLower()))
            {
                var more = GetAllRecursiveProjectFiles(directory);
                csprojFiles.AddRange(more);
            }
        }

        return csprojFiles;
    }
}