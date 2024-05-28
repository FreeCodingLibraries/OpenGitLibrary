using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp.Components.ProjectTree;

internal class CommitNodeMaker
{
    public static MenuItem MakeCommitNode(TNode node)
    {
        var commitNode = new MenuItem();
        commitNode.Tag = node;
        commitNode.Header = "Commit";
        commitNode.Click += (e, a) =>
        {
            var path = Path.Join(node.LocalFolderPath, node.PathName);
            //var sln = Directory.GetFiles(path, "*.sln").FirstOrDefault();
            //if (sln != null)
            Process.Start(CurrentAppConfig.GitExtensionsExePath, $"commit \"{path}\"");
        };
        return commitNode;
    }
}