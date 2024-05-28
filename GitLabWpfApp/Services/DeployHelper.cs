using System.IO;
using System.Windows;

using GitLabLibrary._UserSpecific;

namespace GitLabWpfApp.Services;

internal class DeployHelper
{
    public static void DirCopy(string src, string targetPath)
    {
        if (!Directory.Exists(targetPath))
            Directory.CreateDirectory(targetPath);

        foreach (var filep in Directory.GetFiles(src))
        {
            var fn = Path.GetFileName(filep);
            var dst = Path.Combine(targetPath, fn);
            File.Copy(filep, dst, true);
        }
        foreach (var d in Directory.GetDirectories(src))
        {
            var fn = Path.GetFileName(d);
            var dst = Path.Combine(targetPath, fn);
            DirCopy(d, dst);
        }
    }
    /*
    public static void DirDel(string targetPath)
    {
        if (Directory.Exists(targetPath))
        {
            dirde
            Directory.CreateDirectory(targetPath);
            foreach (var filep in Directory.GetFiles(src))
            {
                var fn = Path.GetFileName(filep);
                var dst = Path.Combine(targetPath, fn);
                File.Copy(filep, dst, true);
            }
            foreach (var d in Directory.GetDirectories(src))
            {
                var fn = Path.GetFileName(d);
                var dst = Path.Combine(targetPath, fn);
                DirCopy(fn, dst);
            }
        }
    }
    */

    public static void Deploy()
    {
        var cSwGitsync = Settings.DeployHelperFolder; 

        try
        {
            if (Directory.Exists(cSwGitsync))
                DeleteInsideFolder(cSwGitsync, true);

            DirCopy(@"C:\Work\GitLabSync\GitLabWpfApp\bin\Debug\net8.0-windows",
                cSwGitsync);

            MessageBox.Show("Deployed!");
        }
        catch (Exception e)
        {
            MessageBox.Show("Can't deploy: " + e.Message);
        }
    }

    private static void DeleteInsideFolder(string target, bool recurse)
    {
        if (Directory.Exists(target))
        {
            foreach (var fp in Directory.GetFiles(target))
            {
                File.Delete(fp);
            }
            foreach (var d in Directory.GetDirectories(target))
            {
                Directory.Delete(d, true);
            }
        }
    }
}