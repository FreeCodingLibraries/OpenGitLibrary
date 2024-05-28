using Microsoft.Extensions.DependencyInjection;

namespace GitLabWpfApp;

internal class ServiceInstaller
{
    private readonly string _jiraSavedfilePath;

    public ServiceInstaller(string jiraSavedfilePath) => _jiraSavedfilePath = jiraSavedfilePath;

    public void Install(ServiceCollection services)
    {
        new GitLabLibrary.DomainInstaller().Install(services);
        new DomainInstaller(_jiraSavedfilePath).Install(services);
    }
}