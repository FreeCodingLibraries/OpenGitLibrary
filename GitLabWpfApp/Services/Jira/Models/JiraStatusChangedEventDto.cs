namespace GitLabWpfApp.Services.Jira.Models;

public class JiraStatusChangedEventDto
{
    public JiraStatus FromStatus { get; set; }
    public JiraStatus ToStatus { get; set; }
    public DateTime Timestamp { get; set; }
}