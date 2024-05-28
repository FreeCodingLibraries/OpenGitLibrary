namespace GitLabWpfApp.Services.Jira.Models;

public enum JiraStatus
{
    Available,
    SelfAssigned,
    InProgress,
    CompletedDev,
    DevTested,
    MRed,
    Merged,
    UatTested,
    Released
}