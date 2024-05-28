using GitLabLibrary;
using GitLabLibrary.Git;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Services.Jira;
using GitLabWpfApp.Services.Jira.Models;
using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp;

public class MainVmDataContextProvider
{
    private AppConfigPersistence _appConfigPersistence;
    private readonly ISolutionTracker _tracker;
    private readonly Libgit2sharpClient _client;
    private readonly IJiraPersistence _jiraPersistence;
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly SearchCtrlViewModel _searchCtrlViewModel;
    private readonly IProjectVmProvider _projectVmProvider;

    public MainVmDataContextProvider(
        AppConfigPersistence appConfigPersistence,
        ISolutionTracker tracker,
        Libgit2sharpClient client,
        IJiraPersistence jiraPersistence,
        MainWindowViewModel mainWindowViewModel,
        SearchCtrlViewModel searchCtrlViewModel,
        IProjectVmProvider ProjectVmProvider
    )
    {
        _appConfigPersistence = appConfigPersistence;
        _tracker = tracker;
        _client = client;
        _jiraPersistence = jiraPersistence;
        _mainWindowViewModel = mainWindowViewModel;
        _searchCtrlViewModel = searchCtrlViewModel;
        _projectVmProvider = ProjectVmProvider;
    }

    public MainWindowViewModel CreateWindowDataContext()
    {
        _mainWindowViewModel.SearchCtrl = _searchCtrlViewModel;
        var _cfg = _appConfigPersistence.LoadConfig();
        var node = new TnodeMaker().Make(_cfg, CurrentAppConfig.CodebaseLocalRootPath);
        var startingnode = node;
       // while (startingnode.NodeName == null)
       // {
       //     startingnode = startingnode.SubNodes[0];
       // }

        try
        {
            var projectNodeViewModels = _projectVmProvider.GetVmProjects(startingnode.SubNodes);
            new FullNameEnhancer().EnchanceWithFullNames(projectNodeViewModels);

            foreach (var vmp in projectNodeViewModels)
            {
                _mainWindowViewModel.Projects.Add(vmp);
            }
        }
        catch (Exception e)
        {
            ;
        }


        //TODO REMOVE
        //var jiraPersistenceModel = _jiraPersistence.LoadJiraData();
     //   var jiraPersistenceModel = new JiraPersistenceModel();
     //   vm.JiraNumbers.Add("DWS-1234");
     //   vm.WorkingOnJiraNumber = "DWS-1234";
     //
     //   foreach (var j in vm.JiraNumbers)
     //   {
     //       jiraPersistenceModel.Tickets.Add(new JiraTicketModel() { JiraName = j, Status = JiraStatus.Available });
     //   }
     //   jiraPersistenceModel.CurrentTicketName = "DWS-1234";
     //
     //   _jiraPersistence.SaveJiraData(jiraPersistenceModel);
      //  var jiraData = _jiraPersistence.LoadJiraData();

        _mainWindowViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(SearchCtrlViewModel.SelectedFilename))
                ;
            //SelectProjectByFilename(viewModelSearchCtrl.SelectedFilename);
        };
        return _mainWindowViewModel;
    }

}