using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GitLabWpfApp.Services.Jira.Models;

namespace GitLabWpfApp.ViewModels;

public partial class JiraDataViewModel : ObservableObject
{
    //   [ObservableProperty] private ObservableCollection<string> _jiraNumbers = new();
    //   [ObservableProperty] private JiraDataObservable _jiraData = (JiraDataObservable)null;
    [ObservableProperty] private ObservableCollection<JiraTicketModelObservable> _tickets  = new();
    [ObservableProperty] private string _currentTicketName;
    [ObservableProperty] private string _selectedTicketName;
    public JiraTicketModelObservable CurrentTicket => _tickets.FirstOrDefault(m => m.JiraName == CurrentTicketName);
    public JiraTicketModelObservable SelectedTicket => _tickets.FirstOrDefault(m => m.JiraName == SelectedTicketName);

    //can remove? 
    partial void OnSelectedTicketNameChanged(string value)
    {
          //  OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedTicketName)));
    }


    public partial class JiraTicketModelObservable : ObservableObject
    {
        [ObservableProperty] private string _jiraName;
        [ObservableProperty] private JiraStatus _status;
        [ObservableProperty] private ObservableCollection<JiraStatusChangedEventObservable> _statusHistory = new();
        [ObservableProperty] private ObservableCollection<string> _relatedProjects = new();
        [ObservableProperty] private string _notes;

    }

    public partial class JiraStatusChangedEventObservable : ObservableObject
    {
        [ObservableProperty] private JiraStatus _fromStatus;
        [ObservableProperty] private JiraStatus _toStatus;
        [ObservableProperty] private DateTime _timestamp;
    }

}