using System.Collections.ObjectModel;

using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Extensions;

internal static class ProjectNodeViewModelExtentions
{
    //public static IList<ObservableCollection<ViewModels.ProjectNodeViewModel>
    public static IList<ProjectNodeViewModel> Flatten(this ObservableCollection<ProjectNodeViewModel> hier)
    {
        var all = new List<ProjectNodeViewModel>();
        
        foreach (var item in hier)
        {
            all.Add(item);
        }
        foreach (var item in hier)
        {
            var flat =             item.Children.Flatten();
            all.AddRange(flat);
        }
        return all;
    }
}