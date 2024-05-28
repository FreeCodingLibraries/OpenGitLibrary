using GitLabLibrary.SimpleBus;
using Microsoft.Extensions.DependencyInjection;

namespace GitLabLibrary;

public class DomainInstaller
{

    public void Install(ServiceCollection services)
    {
        services.AddSingleton<IBusPublisher, MainBusPublisher>();
        services.AddSingleton<IBusSubscriber, MainBusSubscriber>();
        services.AddSingleton<MainBus>();
    }
}