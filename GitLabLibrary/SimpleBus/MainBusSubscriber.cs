
namespace GitLabLibrary.SimpleBus;

public class MainBusSubscriber : IBusSubscriber
{
    private readonly MainBus _bus;

    public MainBusSubscriber(MainBus bus) => _bus = bus;

    public IDisposable Subscribe(string observerName, Action<IMainBusMessage> observerAction)
    {
        return _bus.Subscribe(x =>
        {
            var name = observerName;

            if (x is IMainBusMessage)
                observerAction(x);
        });
    }

    public IDisposable Subscribe(Action<IMainBusMessage> observerAction) => _bus.Subscribe(observerAction);
}