using static System.Net.Mime.MediaTypeNames;
using System.Xml;

namespace NucleusDependencyAnalyser;

internal class PackageExtractor
{
    List<string> _packages = new List<string>();
    public List<string> Extract(string projectContent)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(projectContent);

        XmlElement root = doc.DocumentElement; //get the root element.
        XmlNodeList allItemGroups = doc.GetElementsByTagName("ItemGroup");

        foreach (XmlNode itemGroup in allItemGroups)
        {
            foreach (XmlNode itemGroupPackageNode in itemGroup.ChildNodes)
            {
                if (itemGroupPackageNode.Name == "PackageReference")

                    if (itemGroupPackageNode.Attributes[0].Name == "Include")
                    {
                        var package = itemGroupPackageNode.Attributes[0].Value;
                        _packages.Add(package);
                    }
            }
            //XmlNodeList refs = i.GetElementsByTagName("PackageReference");
            // "PackageReference"
        }
        // Check to see if the element has a RequestType and ReceiptNumber attribute.
        if (root.HasAttribute("Project"))
        {
            var requesttype = root.GetAttribute("Project");
        }

        if (root.HasAttribute("ReceiptNumber"))
        {
            var receiptnumber = root.GetAttribute("ReceiptNumber");
            Console.WriteLine($"Request Type: {receiptnumber}");
        }

        return _packages;
    }
}