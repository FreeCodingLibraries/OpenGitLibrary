using System.Collections;

namespace GitLabLibrary.SimpleBus;

public interface IMainBusMessage
{
}
public class UpdateJiraRelatedProjectsFromTicketSelectionMessage : IMainBusMessage
{
    public List<string> Projects { get; set; }

}


public class UpdateJiraProjectsMessage : IMainBusMessage
{
    public List<string> Projects { get; set; }
}