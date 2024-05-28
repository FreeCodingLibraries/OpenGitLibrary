using GitLabWpfApp.Tnodes;
using ConceptViewModel = GitLabWpfApp.ViewModels.ConceptViewModel;

namespace GitLabWpfApp;

internal class NodeFlattner
{
    private readonly HashSet<Guid> _guids = new();

    public IEnumerable<ConceptViewModel> Flatten(TNode projectMenuNode)
    {
        if (projectMenuNode.Project != null)
            if (!_guids.Contains(projectMenuNode.Project.Id.Value))
            {
                _guids.Add(projectMenuNode.Project.Id.Value);
                yield return projectMenuNode.Project;
            }

        foreach (var sn in projectMenuNode.SubNodes)
        {
            var all = Flatten(sn);
            foreach (var nConceptItem in all) yield return nConceptItem;
        }
    }
}