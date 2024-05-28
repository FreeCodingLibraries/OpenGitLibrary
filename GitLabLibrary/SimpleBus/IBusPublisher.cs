namespace GitLabLibrary.SimpleBus;

public interface IBusPublisher
{
    void Publish(IMainBusMessage message);
}