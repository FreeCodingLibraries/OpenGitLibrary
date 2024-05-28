using System.Configuration;

using GitLabLibrary;
using GitLabLibrary._UserSpecific;
using GitLabLibrary.Git;

using GitLabWpfApp.Framework.Logging;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Services.Jira;
using GitLabWpfApp.SwitchAppHookup;
using GitLabWpfApp.ViewModels;
using GitLabWpfApp.Views;

using Microsoft.Extensions.DependencyInjection;

namespace GitLabWpfApp;

internal class DomainInstaller
{
    private readonly string _jiraSavedfilePath;

    public DomainInstaller(string jiraSavedfilePath) => _jiraSavedfilePath = jiraSavedfilePath;

    public void Install(ServiceCollection services)
    {
        services.AddScoped<WindowFindDemo>();
        services.AddScoped<IAppHookupStuffBuilder, AppHookupStuffBuilder>();
        services.AddScoped<MainWindowViewModel>();
        services.AddScoped<SearchCtrlViewModel>();
        services.AddScoped<WorkRelatedDetailsViewModel>();

        services.AddSingleton<ILogger, Logger>();

        if (Settings.UseFakeProjVmProvider)
            services.AddSingleton<IProjectVmProvider, FakeProjectVmProvider>();
        else
            services.AddSingleton<IProjectVmProvider, ProjectVmProvider>();

        services.AddSingleton<ISolutionTracker, SolutionTracker>();

        services.AddSingleton<ProjectNodeViewModel>();
        services.AddSingleton<SolutionSearchControl>();
        services.AddSingleton<ProjectsMenu>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainVmDataContextProvider>();
        services.AddSingleton<AppConfigPersistence>();
        services.AddScoped<Libgit2sharpClient>();
        services.AddScoped<IJiraPersistence, JiraPersistence>();
        services.AddScoped<IJiraPersistence>(k => new JiraPersistence(_jiraSavedfilePath, k.GetService<ILogger>()));

        // instance
        services.AddTransient<RelatedJiraProjectsViewModel>();
    }
}