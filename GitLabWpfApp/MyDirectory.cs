using System.IO;

namespace GitLabWpfApp;

public class MyDirectory
{
    public static IEnumerable<string> GetAllRecurseFiles(string rootPath, string pattern, SearchOption option,
        IList<string> excludedFolderNames)
    {
        var files = Directory.EnumerateFiles(rootPath, pattern);
        var directories = Directory.GetDirectories(rootPath);
        foreach (var d in directories)
        {
            if (excludedFolderNames.Any(e =>
                {
                    var name = Path.GetFileName(d);
                    return string.Compare(name, e, StringComparison.InvariantCultureIgnoreCase) == 0;
                }))
                continue;

            var morefiles = GetAllRecurseFiles(d, pattern, option, excludedFolderNames);
            files = files.Concat(morefiles);
        }

        return files;
    }

    public static IEnumerable<string> EnumerateFiles(string rootPath, string pattern, SearchOption option)
    {
        return GetAllRecurseFiles(rootPath, pattern, option,
            new List<string> { ".git", ".vs", ".vscode", "bin", "obj", "packages" });

        //return Directory.EnumerateFiles(rootPath, pattern, option);
    }
}