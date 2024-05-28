using GitLabWpfApp.Services.Jira.Models;

namespace GitLabWpfApp.Services.Jira;

public interface IJiraPersistence
{
    JiraDataDto LoadJiraData();
    void SaveJiraData(JiraDataDto model);
}