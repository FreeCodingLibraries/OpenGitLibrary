using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp.Processing;

internal class VsOpener
{
    public static MenuItem MakeOpeninVs(TNode node)
    {
        var openVs = new MenuItem();
        openVs.Tag = node;
        openVs.Header = "VS2022";
        openVs.Click += (e, a) =>
        {
            var path = Path.Join(node.LocalFolderPath, node.PathName);
            var sln = Directory.GetFiles(path, "*.sln").FirstOrDefault();
            if (sln != null)
                Process.Start("explorer.exe", sln);
        };
        // 
        return openVs;
    }
}