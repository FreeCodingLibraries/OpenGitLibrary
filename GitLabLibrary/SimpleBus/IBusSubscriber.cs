namespace GitLabLibrary.SimpleBus;

public interface IBusSubscriber
{
    IDisposable Subscribe(string observerName, Action<IMainBusMessage> observerAction);
}