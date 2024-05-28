using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;

using GitLabLibrary._UserSpecific;
using GitLabLibrary.Git;
using GitLabLibrary.SimpleBus;
using GitLabLibrary.SolutionMaker.Basics;
using GitLabLibrary.SolutionMaker.Builders;

using GitLabWpfApp.Extensions;
using GitLabWpfApp.Framework.Logging;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Services.Jira;
using GitLabWpfApp.Services.Jira.Models;

using static GitLabWpfApp.ViewModels.JiraDataViewModel;

namespace GitLabWpfApp.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private readonly ILogger _logger;
    private readonly Libgit2sharpClient _client;
    private readonly IJiraPersistence _jiraPersistence;
    private readonly IBusPublisher _publisher;
    private readonly ISolutionTracker _solutionTracker;
    [ObservableProperty] private JiraDataViewModel _jiraData;

    [ObservableProperty] private RelatedJiraProjectsViewModel _jiraProjects = new RelatedJiraProjectsViewModel();
    [ObservableProperty] private ObservableCollection<ProjectNodeViewModel> _projects = new();
    [ObservableProperty] private SearchCtrlViewModel _searchCtrl;
    [ObservableProperty] private ObservableCollection<ProjectNodeViewModel> _selectedNodes = new();
    [ObservableProperty] private ProjectNodeViewModel _selectedProject;
    [ObservableProperty] private WorkRelatedDetailsViewModel _workRelatedDetails = new();

    private CancellationTokenSource source = new();
    public string ID { get;  }

    public MainWindowViewModel()
    {
        ID = "INVALID (wrong constructor used)";
    }


    // Purely for UI Design purposes (need a constructor)


    public MainWindowViewModel(
        Libgit2sharpClient client,
        ISolutionTracker solutionTracker,
        IJiraPersistence jiraPersistence,
        IBusPublisher publisher,
        ILogger logger
        )
    {
        ID = new Random().Next(100000 - 1).ToString("00000");
        // Dependencies
        _logger = logger;
        _client = client;
        _solutionTracker = solutionTracker;
        _jiraPersistence = jiraPersistence;
        _publisher = publisher;

        // Resolve sub viewmodels
        _workRelatedDetails = App.Resolve<WorkRelatedDetailsViewModel>();

        // The rest
        JiraDataDto jiraDataDto = jiraPersistence.LoadJiraData();
        _jiraProjects.OnProjectsDeleted += (sender, args) =>
        {
            _logger.Log($"OnProjectsDeleted");
            UpdateSelectedTicketToSetRelatedProjects(_jiraProjects.Projects);
            SaveJiraData();
        };
        _jiraProjects.OnProjectsDropped += (sender, items) =>
        {
            if (_jiraData.SelectedTicket == null)
                return;

            _logger.Log($"OnProjectsDropped");
            foreach (var it in items)
            {

                if (it != null && !_jiraProjects.Projects.Any(pnvm => pnvm == it))
                {
                    _jiraProjects.Projects.Add(it);
                }

            }

            UpdateSelectedTicketToSetRelatedProjects(items);
            SaveJiraData();
        };
        _jiraData = jiraDataDto.Map();

        //_jiraData = new JiraDataViewModel(jiraDataDto);
        string tickname = _jiraData.CurrentTicketName;
        _jiraData.CurrentTicketName = "";
        _jiraData.Tickets.CollectionChanged += (sender, args) =>
        {
            _logger.Log("_jiraData.Tickets.CollectionChanged");
            jiraPersistence.SaveJiraData(_jiraData.Map());
        };
        bool isInitialising = true;
        _jiraData.CurrentTicketName = tickname;
        _jiraData.PropertyChanged += (sender, args) =>
        {
            _logger.Log($"_jiraData.PropertyChanged {args.PropertyName}");

            if (isInitialising)
            {
                return;
            }


            if (args.PropertyName == nameof(JiraDataViewModel.SelectedTicketName))
            {
                // Handle the change of SelectedItem in the parent view model
                OnJiraDataSelectedTicketChanged(JiraData.SelectedTicket);
                return;
            }


            if (args.PropertyName == nameof(JiraDataViewModel.SelectedTicketName))
            {
                ObservableCollection<string> allRelatedProjects = _jiraData.CurrentTicket.RelatedProjects;
                _publisher.Publish(new UpdateJiraRelatedProjectsFromTicketSelectionMessage
                {
                    Projects = allRelatedProjects.ToList()
                });
            }

            jiraPersistence.SaveJiraData(_jiraData.Map());

            if (args.PropertyName == nameof(JiraDataViewModel.CurrentTicketName))
            {
                source.Cancel();
                source = new CancellationTokenSource();
                CancellationToken cancellationToken = source.Token;

                Task task = RecalculateProjectGitStatussesAsync(cancellationToken);
                task.ConfigureAwait(false);

                //  var task = Task.Run(() => RecalculateProjectGitStatussesAsync(cancellationToken));
                //  task.ConfigureAwait(true);
            }
        };
        Projects.CollectionChanged += (sender, e) =>
        {
            if (e.OldItems != null)
            {
                foreach (object? oldItem in e.OldItems)
                {
                    (oldItem as ProjectNodeViewModel).PropertyChanged -= Pchanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (object? item in e.NewItems)
                {
                    (item as ProjectNodeViewModel).PropertyChanged += Pchanged;
                }
            }
        };
        isInitialising = false;
    }

    private void UpdateSelectedTicketToSetRelatedProjects(IEnumerable<string> jiraProjectsProjects)
    {
        if (_jiraData.SelectedTicket == null)
            return;

        _logger.Log($"UpdateSelectedTicketToSetRelatedProjects: {string.Join(",", jiraProjectsProjects)}");
        _jiraData.SelectedTicket.RelatedProjects.Clear();
            foreach (var x in jiraProjectsProjects)
            _jiraData.SelectedTicket.RelatedProjects.Add(x);
    }

    private void SetJiraRelatedProjects(ObservableCollection<string> jiraProjectsProjects)
    {
        JiraProjects.Projects.Clear();
        foreach (var project in jiraProjectsProjects)
        { JiraProjects.Projects.Add(project); }
        _logger.Log($"Setting JiraProjects component to show: {string.Join(",", JiraProjects.Projects)}");

    }

    public void SaveJiraData()
    {
        JiraDataDto jiraDataDto = _jiraData.Map();
        _jiraPersistence.SaveJiraData(jiraDataDto);
    }

    private void OnJiraDataSelectedTicketChanged(JiraTicketModelObservable item)
    {
        _logger.Log("Selected jira ticket changed");
        if (item != null)
        {
            SetJiraRelatedProjects(item.RelatedProjects);
        }
    }

    private void Pchanged(object? sender, PropertyChangedEventArgs e)
    {
        // _logger.Log($"_jiraData.Project[?] ItemChanged {e.PropertyName}");

        if (e.PropertyName == nameof(ProjectNodeViewModel.IsSelected))
        {
            ProjectNodeViewModel? node = sender as ProjectNodeViewModel;
            if (node != null)
            {
                UpdateSelecte(node, node.IsSelected);
            }
        }
    }


    public async Task RecalculateProjectGitStatussesAsync(CancellationToken cancellationToken)
    {
        string branch = $"feature/{JiraData.CurrentTicketName}";
        List<Task> tasks = new();
        IList<ProjectNodeViewModel> projectNodeViewModels = Projects.Flatten();
        IList<ProjectNodeViewModel>
            filteredSelectionOfProjects = projectNodeViewModels; //.Where(m=>m.Details.Visible);

        Dictionary<string, GitSyncStatus> dict = filteredSelectionOfProjects
            .Select(m => m.FullPath)
            .Distinct()
            .ToDictionary(m => m, _ => GitSyncStatus.updateing);

        foreach (KeyValuePair<string, GitSyncStatus> entry in dict)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Task t = Task.Run(() =>
            {
                string t = entry.Key;
                string combine = Path.Combine(t, ".git");
                if (Directory.Exists(combine))
                {
                    try
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        string branchName = _client.GetBranchName(t);
                        dict[entry.Key] = branchName == branch ? GitSyncStatus.synced
                            : branchName == "master" ? GitSyncStatus.master : GitSyncStatus.outOfSync;
                    }
                    catch (Exception e)
                    {
                        dict[entry.Key] = GitSyncStatus.nonRepo;
                    }
                }
                else
                {
                    dict[entry.Key] = GitSyncStatus.nonRepo;
                }

                _ = projectNodeViewModels
                    .Where(m => m.FullPath == entry.Key)
                    .Select(m =>
                    {
                        m.IsInGitSyncWithJiraNumber = dict[entry.Key];
                        return true;
                    }).ToList();
            }, cancellationToken);
            tasks.Add(t);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            await Task.WhenAll(tasks);

            foreach (ProjectNodeViewModel node in filteredSelectionOfProjects)
            {
                node.IsInGitSyncWithJiraNumber = dict[node.FullPath];
            }
        }
    }


    public void DblClickOnProject(ProjectNodeViewModel selectedTreeItem)
    {
        OpenFirstSolutionUnderFolder(selectedTreeItem.FullPath);
    }

    private void OpenFirstSolutionUnderFolder(string path1)
    {
        string? sln = Directory.GetFiles(path1, "*.sln").FirstOrDefault();
        if (sln != null)
        {
            if (_solutionTracker.HasSolution(sln))
            {
                _solutionTracker.BringUp(sln);
            }
            else
            {
                _solutionTracker.Start(sln);
            }
        }
    }

    public void UpdateSelecte(ProjectNodeViewModel node, bool isSelected)
    {
        _logger.Log($"_jiraData.SelectedNodesChanged");

        if (isSelected)
        {
            if (!SelectedNodes.Contains(node))
            {
                SelectedNodes.Add(node);
            }
        }
        else
        {
            if (SelectedNodes.Contains(node))
            {
                SelectedNodes.Remove(node);
            }
        }
    }

    public void OpenSolutionForJira(string jira)
    {
        ProcessStarter.OpenSolutionForJira(_solutionTracker, jira);
    }

    public void OpenSingleProjectSolutionByFullName(string selectedItemProjectFullname)
    {
        ProjectNodeViewModel selectedTreeItem = Projects.Flatten()
            .FirstOrDefault(m => m.FullName == selectedItemProjectFullname);
        if (selectedTreeItem != null)
        {
            DblClickOnProject(selectedTreeItem);
        }
    }

    public void CreateSolution(string jiraDataSelectedTicketName, IEnumerable<string> relatedProjPaths)
    {
        var SrcProjAbsoluteFolder = Settings.SrcProjAbsoluteFolder; 

        if (string.IsNullOrEmpty(jiraDataSelectedTicketName))
            return;

        var slnFile = $"{jiraDataSelectedTicketName}.sln";
        var saveLocation = Path.Combine(SrcProjAbsoluteFolder, slnFile);

        var creator = new SolutionCreator();
        SolutionFolder nfolder = null;
        Predicate<string> filepathcondition = x =>
        {
            return true;//x.ToLower().Contains("reference");
        };
        var groupByDepth = 0;
        //var groupByDepth = 1;
        var builder = creator;
        foreach (var projPath in relatedProjPaths)
        {
            builder = builder.AddProjectsUnder
                (projPath, groupByDepth, nfolder, filepathcondition);
        }
        builder.WriteTo(saveLocation);
    }

    public void SwitchToPreviousWindow()
    {
        SwitchToPreviousWindowHandler.Invoke(this, null);
    }

    public event EventHandler SwitchToPreviousWindowHandler;
}
