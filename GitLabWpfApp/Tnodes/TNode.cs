using System.Diagnostics;
using System.IO;
using GitLabLibrary;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Tnodes;

[DebuggerDisplay("{NodeName}")]
public class TNode
{
    public TNode(string pathName, RawConceptItem item, string nodeName, string localFolderPath)
    {
        PathName = pathName;
        LocalFolderPath = localFolderPath;
        Project = item.ToNotify();
        NodeName = pathName;
    }

    public string NodeName { get; set; }

    public string LocalFolderPath { get; }
    public string PathName { get; }
    public List<TNode> SubNodes { get; } = new();

   // public List<ConceptViewModel> Projects { get; } = new();
    public ConceptViewModel Project { get; set; }

    //was: public void Add(string[] paths, GitLabProjectContext item)


    public void Add(string[] paths, RawConceptItem item, string name, string localfolderpath)
    {
        var first = paths.First();
        var node = SubNodes.FirstOrDefault(m => m.PathName == first);
        if (node == null)
        {
            node = new TNode(first, null, name, localfolderpath);
            node.FullPath = localfolderpath;
            SubNodes.Add(node);
        }

        if (paths.Length == 1)
        {
            node.FullPath = Path.Join(localfolderpath, paths.First());
            node.Project = item.ToNotify();
        }
        else
            node.Add(paths.Skip(1).ToArray(), item, name, Path.Join(localfolderpath, first));
    }

    public string FullPath { get; set; }
}