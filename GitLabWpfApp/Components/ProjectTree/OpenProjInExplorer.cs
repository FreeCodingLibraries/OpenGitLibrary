using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp.Components.ProjectTree;

internal class OpenProjInExplorer
{
    public static MenuItem MakeOpenProjInExplorer(TNode node)
    {
        var openProj = new MenuItem();
        openProj.Tag = node;
        openProj.Header = "Explore";
        openProj.Click += (e, a) =>
        {
            var path = Path.Join(node.LocalFolderPath, node.PathName);
            var arg = "\"" + path + "\"";
            //var arg = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", arg);
        };
        return openProj;
    }
}