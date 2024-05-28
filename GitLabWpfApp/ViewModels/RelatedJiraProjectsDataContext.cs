using System.Collections.ObjectModel;

namespace GitLabWpfApp.ViewModels
{
    public class RelatedJiraProjectsViewModel
    {
        public EventHandler OnProjectsDeleted;

        public EventHandler<string[]> OnProjectsDropped;

        public ObservableCollection<string> Projects { get; set; } = new();
    }
}