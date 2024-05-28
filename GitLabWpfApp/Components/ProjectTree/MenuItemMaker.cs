using System.Diagnostics;
using System.Windows.Controls;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp.Components.ProjectTree;

internal class MenuItemMaker
{
    public static MenuItem MakeMenuItem(TNode node)
    {
        var mR = new MenuItem();
        mR.Tag = node;
        mR.Header = "MRs";
        mR.Click += (e, a) =>
        {
            //https://gitlabUrl/rootcodepath/projname/domains/my-data/my-data-service/-/branches

            var pC = node.Projects.FirstOrDefault();
            var url = pC.GitHttpUrl;
            //var path = Path.Join(node.LocalFolderPath, node.PathName);
            //https://gitlabUrl/rootcodepath/projname/domains/other/other-service/-/merge_requests/85
            var arg = $"\"{url}/-/merge_requests/\"";
            //var arg = "/select, \"" + path + "\"";
            Process.Start("explorer.exe", arg);
        };
        // 
        return mR;
    }
}