using System.Collections.ObjectModel;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp;

internal class FakeProjectVmProvider : IProjectVmProvider
{
    private readonly ISolutionTracker _solutionTracker;

    public FakeProjectVmProvider(ISolutionTracker solutionTracker)
    {
        _solutionTracker = solutionTracker;
    }

    public List<ProjectNodeViewModel> GetVmProjects(List<TNode> nodes)
    {

        var domains = new ObservableCollection<ProjectNodeViewModel>()
        {
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "DomProject1",
            },
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "DomProject2"
            },
        };
        var frameworks = new ObservableCollection<ProjectNodeViewModel>()
        {
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "Framework1"
            },
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "Framework2"
            },
        };


        var projectNodeViewModels = new List<ProjectNodeViewModel>()
        {
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "Domains",
                Children = domains
            },
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "Frameworks",
                Children = frameworks
            },
            new ProjectNodeViewModel(_solutionTracker)
            {
                DisplayName = "Deployment"
            },
        };

        return projectNodeViewModels;
    }

}