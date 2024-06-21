namespace NucleusDependencyAnalyser;

internal class ContextBuilder
{
    public CsprojContext Build(string filepath)
    {
        var txt = File.ReadAllText(filepath);
        var isOldXml = txt.StartsWith("<?xml");

        if (isOldXml)
        {
            var context = new CsprojContext(filepath, isOldXml, null);
            return context;
        }
        else
        {
            var packages = new PackageExtractor().Extract(txt);
            var context = new CsprojContext(filepath, isOldXml, packages);
            return context;
        }
    }
}