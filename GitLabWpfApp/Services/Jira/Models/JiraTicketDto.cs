using System.Reactive.Linq;

namespace GitLabWpfApp.Services.Jira.Models;

public class JiraTicketDto
{
    public string JiraName { get; init; }
    public JiraStatus Status { get; set; }
    public List<JiraStatusChangedEventDto> StatusHistory { get; set; }
    public List<string> RelatedProjects { get; set; }
    public string Notes { get; set; }

}