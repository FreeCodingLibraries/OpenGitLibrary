using System.Reactive.Subjects;

namespace GitLabLibrary.SimpleBus;

public class MainBus
{
    private readonly Subject<IMainBusMessage> _bus = new();

    public IDisposable Subscribe(Action<IMainBusMessage> observerAction) => _bus.Subscribe(observerAction);

    public void Emmit(IMainBusMessage message)
    {
        _bus.OnNext(message);
    }
}