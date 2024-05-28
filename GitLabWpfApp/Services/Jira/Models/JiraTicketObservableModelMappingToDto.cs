using System.Collections.ObjectModel;
using GitLabWpfApp.ViewModels;
using static GitLabWpfApp.ViewModels.JiraDataViewModel;

namespace GitLabWpfApp.Services.Jira.Models;

public static class JiraTicketObservableModelMappingToDto
{
    public static JiraDataDto Map(this JiraDataViewModel input)
    {
        return new JiraDataDto ()
        {
            CurrentTicketName = input.CurrentTicketName,
            Tickets = input.Tickets.Select(Map).ToList(),
        };
    }

    public static JiraTicketDto  Map(this JiraDataViewModel.JiraTicketModelObservable input)
    {
        var inputRelatedProjects = input.RelatedProjects?.ToList() ?? Array.Empty<string>().ToList();
        var jiraStatusChangedEvents = input.StatusHistory?.Select(Map).ToList() ?? Array.Empty<JiraStatusChangedEventDto>().ToList();
        return new JiraTicketDto()
        {
            StatusHistory = jiraStatusChangedEvents,
            JiraName = input.JiraName,
            Notes = input.Notes,
            RelatedProjects = inputRelatedProjects,
            Status = input.Status
        };
    }

    public static JiraStatusChangedEventDto Map(this JiraDataViewModel.JiraStatusChangedEventObservable input)
    {
        return new JiraStatusChangedEventDto()
        {
            FromStatus = input.FromStatus,
            ToStatus= input.ToStatus,
            Timestamp = input.Timestamp,
        };
    }

}