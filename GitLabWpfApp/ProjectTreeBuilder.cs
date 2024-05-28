using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using GitLabWpfApp.Components.ProjectTree;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp;

internal class ProjectTreeBuilder
{
    private readonly ISolutionTracker _solutionTracker;

    public ProjectTreeBuilder(ISolutionTracker solutionTracker)
    {
        _solutionTracker = solutionTracker;
    }

    /*public void Bindtree(TNode nodes)
    {
        var tree = BuildTree(nodes.SubNodes[0]);
        return tree;
    }*/
    /*
    public void Bindtree(TreeView treeView1)
     {
         var localCodebaseRootPath = CurrentAppConfig.CodebaseLocalRootPath;
         var appConfigPersistence = new AppConfigPersistence();
         AppConfiguration config = appConfigPersistence.LoadConfig();
         var nodes = new TnodeMaker().Make(config, localCodebaseRootPath);

         var tree = BuildTree(nodes.SubNodes[0]);
         treeView1.Items.Clear();
         treeView1.Items.Add(tree);
     }
     */


    public TreeViewItem BuildTree(TNode node)
    {
        var endsToExpand = new List<string>
        {
            "root-path-here", "subfolder", "domains"
        };
        var resultTreeviewItem = new TreeViewItem();

        resultTreeviewItem.Header = node.PathName;
        resultTreeviewItem.IsExpanded = false;
        if (endsToExpand.Any(e => node.PathName?.EndsWith(e) ?? true))
            resultTreeviewItem.IsExpanded = true;


        var isNodeLeave = !node.SubNodes.Any();
        if (isNodeLeave)
            resultTreeviewItem.MouseDoubleClick += (e, a) =>
            {
                var path1 = Path.Join(node.LocalFolderPath, node.PathName);
                var sln = Directory.GetFiles(path1, "*.sln").FirstOrDefault();
                if (sln != null)
                {
                    if (_solutionTracker.HasSolution(sln))
                        _solutionTracker.BringUp(sln);
                    else
                        _solutionTracker.Start(sln);
                }

                return;
                var path = Path.Join(node.LocalFolderPath, node.PathName);
                var arg = "\"" + path + "\"";
                //var arg = "/select, \"" + path + "\"";
                Process.Start("explorer.exe", arg);
            };

        var x = node.SubNodes
            .OrderBy(m => m.PathName).ToArray();
        // .Select(BuildTree);

        foreach (var si in x)
        {
            var i = BuildTree(si);
            resultTreeviewItem.Items.Add(i);
            // i.MouseDoubleClick += (e, a) => {
            //         Process.Start("explorer.exe", sn.LocalRepoPath);
            // }
            ;
        }

   //     var contextMenu = new ContextMenu();
   //     contextMenu.Tag = node;
   //     contextMenu.Items.Add(HeaderMaker.MakeHeader(node));
   //     contextMenu.Items.Add(OpenProjInExplorer.MakeOpenProjInExplorer(node));
   //     if (isNodeLeave)
   //     {
   //         contextMenu.Items.Add(MenuItemMaker.MakeMenuItem(node));
   //         contextMenu.Items.Add(VsOpener.MakeOpeninVs(node));
   //         contextMenu.Items.Add(CommitNodeMaker.MakeCommitNode(node));
   //         contextMenu.Items.Add(TeamCityNodeMaker.MakeTC(node));
   //     }
   //
   //
   //     resultTreeviewItem.ContextMenu = contextMenu;
        resultTreeviewItem.DataContext = node;

        //item.Items.Add("Monitor");
        //item.Items.Add("LapTop");
        //
        //treeView1.Items.Add(item);

        return resultTreeviewItem;
    }
}