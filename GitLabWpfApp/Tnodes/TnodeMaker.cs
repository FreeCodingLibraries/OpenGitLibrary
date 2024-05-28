using GitLabLibrary;

namespace GitLabWpfApp.Tnodes;

public class TnodeMaker
{
    public TNode Make(AppConfiguration config, string localPath)
    {
        var separator = "/";

        var result = new TNode(null, null, "PROJECTS", localPath);
        var sortedProjects = config.Projects?.OrderBy(m=>m.PathWithNs);
        if (sortedProjects != null)
        {
            foreach (var proj in sortedProjects)
                if (proj.Visible)
                {
                    var rPath = proj.PathWithNs;
                    var paths = rPath.Split(separator);
                    result.Add(paths, proj, proj.Name, localPath);
                }
        }

        return result;
    }
    /*
    public TNode Make(IEnumerable<AdditionalRepoInfo> results, string localPath)
    {
        var separator = "/";

        var result = new TNode(null, localPath);
        foreach (var r in results)
        {
            var rPath = r.GitLabProjectContext.path_with_namespace;
            var paths = rPath.Split(separator);
            result.Add(paths, r, localPath);
        }

        return result;
    }
*/
}