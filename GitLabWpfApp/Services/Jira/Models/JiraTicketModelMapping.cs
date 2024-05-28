using System.Collections.ObjectModel;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Services.Jira.Models;

public static class JiraTicketModelMapping
{
    public static JiraDataViewModel Map(this JiraDataDto input)
    //public JiraDataViewModel(this JiraPersistenceModel jiraPersistenceModel)
    {
//        foreach (var j in input.Tickets)
//            _tickets.Add(j.Map());
//        _currentTicketName = ;
        return new JiraDataViewModel()
        {
            CurrentTicketName = input.CurrentTicketName,
            Tickets = new ObservableCollection<JiraDataViewModel.JiraTicketModelObservable>(input.Tickets.Select(Map))
        };
    }

    public static JiraDataViewModel.JiraTicketModelObservable Map(this JiraTicketDto input)
    {
        var inputRelatedProjects = input.RelatedProjects ?? Array.Empty<string>().ToList();
        var jiraStatusChangedEvents = input.StatusHistory ?? Array.Empty<JiraStatusChangedEventDto>().ToList();
        return new JiraDataViewModel.JiraTicketModelObservable()
        {
            StatusHistory = new ObservableCollection<JiraDataViewModel.JiraStatusChangedEventObservable>(jiraStatusChangedEvents.Select(Map)),
            JiraName = input.JiraName,
            Notes = input.Notes,
            RelatedProjects = new ObservableCollection<string>( inputRelatedProjects),
            Status = input.Status
        };
    }
    public static JiraDataViewModel.JiraStatusChangedEventObservable Map(this JiraStatusChangedEventDto input)
    {
        return new JiraDataViewModel.JiraStatusChangedEventObservable()
        {
            FromStatus = input.FromStatus,
            ToStatus= input.ToStatus,
            Timestamp = input.Timestamp,
        };
    }

}