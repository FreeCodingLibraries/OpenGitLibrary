namespace NucleusDependencyAnalyser;

internal class ProjLocator
{
    private readonly string _srcFolder;

    public ProjLocator(string srcFolder)
    {
        _srcFolder = srcFolder;
    }

    public string[] FindCsProjs()
    {
        //var extensions = new List<string> { ".csproj" };
        string[] files = Directory.GetFiles(_srcFolder, "*.csproj", SearchOption.AllDirectories)
            //.Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0)
            .ToArray();

        return files;
    }
}