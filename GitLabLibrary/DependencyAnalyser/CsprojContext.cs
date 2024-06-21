namespace NucleusDependencyAnalyser;

internal class CsprojContext
{
    public readonly string FilePath;
    public readonly bool IsOldXml;
    public readonly List<string> Packages;

    public CsprojContext(string filePath, bool isOldXml, List<string> packages)
    {
        FilePath = filePath;
        IsOldXml = isOldXml;
        Packages = packages;
    }
}