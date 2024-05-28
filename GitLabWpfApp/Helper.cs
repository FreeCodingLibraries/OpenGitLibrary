using GitLabLibrary;
using ConceptViewModel = GitLabWpfApp.ViewModels.ConceptViewModel;

namespace GitLabWpfApp;

public static class Helper
{
    public static ConceptViewModel ToNotify(this RawConceptItem item)
    {
        if (item == null)
            return null;

        return new ConceptViewModel
        {
            Id = item.Id,
            DefaultBranch = item.DefaultBranch,
            GitHttpUrl = item.GitHttpUrl,
            Name = item.Name,
            NameWithNs = item.NameWithNs,
            PathWithNs = item.PathWithNs,
            TcHttpUrl = item.TcHttpUrl ?? "<DEFAULT>",
            Visible = item.Visible
        };
    }
}