using System.Windows;

using GitLabWpfApp.Framework.Logging;
using GitLabWpfApp.SwitchAppHookup;
using GitLabWpfApp.ViewModels;
using GitLabWpfApp.Views;

using Microsoft.Extensions.DependencyInjection;

namespace GitLabWpfApp
{
    public partial class App : Application
    {
        private static ServiceCollection _servicesRegistration;
        private readonly AppHookupStuff _appHookupStuff;
        private readonly ILogger? _logger;

        public App()
        {
            string jiraSavedfilePath = "jiraData.json";

            // Build Services as container
            _servicesRegistration = new ServiceCollection();
            new ServiceInstaller(jiraSavedfilePath).Install(_servicesRegistration);
            Services = _servicesRegistration.BuildServiceProvider();

            // Instantiate Main Window
            MainVmDataContextProvider mainVmDataContextProvider = Services.GetService<MainVmDataContextProvider>();
            var _appHookupStuffBuilder = Services.GetService<IAppHookupStuffBuilder>();

            _logger = Services.GetService<ILogger>();
             //_logger.OpenBaretail();
            //Services.GetService<WindowFindDemo>().GoTo();
            _logger.Log("STARTING UP");
            MainWindow window = Services.GetService<MainWindow>();
            MainWindowViewModel viewModel = mainVmDataContextProvider.CreateWindowDataContext();
            window.Show();

            _appHookupStuff = _appHookupStuffBuilder
                .ExcludeThreadByWindow(window)
                .Build();

            //Hookup events (app switch / keepass)

            _appHookupStuff.Setup();
            viewModel.SwitchToPreviousWindowHandler += (o, o1) =>
            {
                _appHookupStuff.SwitchToLastActiveApp();
            };
            window.DataContext = viewModel;
            //window.WorkRelatedDetailsTab.DataContext = viewModel;
            window.WorkRelatedDetailsViewItem.DataContext = viewModel;

            window.Closed += (sender, args) =>
            {
                SafelyTearDown();
                Environment.Exit(0);
            };
            Exit += App_Exit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }


        public static IServiceProvider Services { get; set; }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            SafelyTearDown();
            // Perform any necessary cleanup here
            // e.ApplicationExitCode contains the exit code
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SafelyTearDown();
            // Handle the unhandled exception
            Exception ex = (Exception)e.ExceptionObject;

            // Log the exception, show a message box, etc.
            MessageBox.Show($"Fatal error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Terminate the application
            Environment.Exit(1);
        }

        private void SafelyTearDown()
        {
            _appHookupStuff.Dispose();
        }

        public static T Resolve<T>()
        {
            T item = Services.GetService<T>();
            if (item == null)
            {
                if (!IsServiceRegistered<T>(_servicesRegistration))
                {
                    throw new Exception($"No registration found for {typeof(T).FullName}");
                }

                throw new Exception("Resolve as (null). Registered?");

                throw new Exception("Resolve as (null). Registered?");
            }

            return item;
        }

        public static bool IsServiceRegistered<T>(IServiceCollection services)
        {
            return services.Any(sd => sd.ServiceType == typeof(T));
        }
    }
}