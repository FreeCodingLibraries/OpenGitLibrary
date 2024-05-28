using Newtonsoft.Json;

namespace GitLabWpfApp.Services.Jira.Models;

public class JiraDataDto
{
    public List<JiraTicketDto> Tickets { get; init; } = new();
    public string CurrentTicketName { get; set; }

    [JsonIgnore]
    public JiraTicketDto CurrentTicket => Tickets.Any(x => x.JiraName == CurrentTicketName)
        ? Tickets.First(x => x.JiraName == CurrentTicketName)
        : null;
}