namespace GitLabLibrary.SimpleBus;

public class MainBusPublisher : IBusPublisher
{
    private readonly MainBus _bus;

    public MainBusPublisher(MainBus bus) => _bus = bus;

    public void Publish(IMainBusMessage message)
    {
        _bus.Emmit(message);
    }
}