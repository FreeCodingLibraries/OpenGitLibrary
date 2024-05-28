using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp;

public interface IProjectVmProvider
{
    List<ProjectNodeViewModel> GetVmProjects(List<TNode> nodes);
}

public class ProjectVmProvider : IProjectVmProvider
{
    const string separator = @"\";

    private Stack<string> _path = new();
    public List<ProjectNodeViewModel> GetVmProjects(List<TNode> nodes)
    {
        var vmProjects = new List<ProjectNodeViewModel>();
        var curpath = string.Join(separator, _path.ToArray());

        foreach (var node in nodes)
        {
            var projectNodeViewModel = new ProjectNodeViewModel(null)
            {
                Details = node.Project,
                DisplayName = node.PathName,
                FullPath = node.FullPath

            };
            if (node.SubNodes != null && node.SubNodes.Any())
            {
                _path.Push(node.NodeName);
                var subs = GetVmProjects(node.SubNodes);
                _path.Pop();

                foreach (var sub in subs)
                {
                    projectNodeViewModel.Children.Add(sub);
                }
                if (node.SubNodes[0].SubNodes != null && node.SubNodes[0].SubNodes.Any())
                    projectNodeViewModel.IsExpanded = true;
            }


            vmProjects.Add(projectNodeViewModel);

        }

        return vmProjects;
    }

}