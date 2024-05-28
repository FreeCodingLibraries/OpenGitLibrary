using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GitLabLibrary;
using GitLabLibrary.SimpleBus;

using GitLabWpfApp.Framework.Logging;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Services;
using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;
using GitLabWpfApp.Views.Help;

using WindowsInput.Native;
using WindowsInput;
using GitLabLibrary._UserSpecific;

namespace GitLabWpfApp.Views;

public partial class MainWindow : Window
{
    private readonly AppConfigPersistence _appConfigPersistence;
    private readonly AppConfiguration _cfg;
    private HelpForMainWindow _helpForMainWindow = new HelpForMainWindow();
    private MainWindowViewModel ViewModel => _viewModel;
    MainWindowViewModel _viewModel = null;


    public MainWindow(IBusSubscriber busSubscriber, ILogger logger)
    {
        logger.Log("Initiated!");

        busSubscriber.Subscribe("mainView", BusMessageReceived);
        try
        {
            InitializeComponent();
            // WorkRelatedDetailsTab.IsSelected = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        DataContextChanged += (sender, args) =>
        {
            if (args.NewValue is MainWindowViewModel x)
            {
                _viewModel = x;
                _viewModel.SwitchToPreviousWindowHandler += (o, o1) =>
                {
                    ;
                };
            }
        };
        WindowState = WindowState.Maximized;
        KeyDown += (a, b) =>
        {
            if (b.Key == Key.F && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (SearchTab.IsSelected)
                {
                    FolderTab.IsSelected = true;
                }
                else
                {
                    SearchTab.IsSelected = true;
                    SrcCtrl.Focus();
                }
            }
        };
        //Closed += (sender, args) => base.closEnvironment.Exit(0);

    }

    private void BusMessageReceived(IMainBusMessage obj)
    {
        var notInitialised = ViewModel.JiraData == null;
        if (notInitialised)
            return;

        if (obj is UpdateJiraProjectsMessage m)
        {
            ViewModel.JiraData.CurrentTicket.RelatedProjects.Clear();
            foreach (var nn in m.Projects)
            {
                ViewModel.JiraData.CurrentTicket.RelatedProjects.Add(nn);
            }
        }
        //todo: save
        ViewModel.SaveJiraData();
    }

    private void SelectProjectByFilename(string selectedFilename)
    {
        var treeView1Item = ProjMenu.treeView1.Items[0] as TreeViewItem;
        var treeView1Item2 = treeView1Item.Items[0] as TreeViewItem;
        var treeView1Item3 = treeView1Item2.Items;
        NavToSubFolders(treeView1Item3, selectedFilename.Split('\\'));
    }

    private void NavToSubFolders(ItemCollection items, string[] split)
    {
        var first = split.First();
        var rest = split.Skip(1);

        foreach (TreeViewItem treeView1Item in items)
        {
            var dataContext = treeView1Item.DataContext as TNode;
            if (dataContext.PathName == first)
            {
                treeView1Item.IsExpanded = true;
                treeView1Item.IsSelected = true;
                NavToSubFolders(treeView1Item.Items, rest.ToArray());

                //FrameworkElement element = treeView1Item as FrameworkElement;
                //element.BringIntoView();
                //element.Focus();
                break;
            }
        }
    }

    private void ButtonOpenAllApps_OnClick(object sender, RoutedEventArgs e)
    {
        AllAppsOpener.OpenAll();
    }

    /// <summary>
    ///   Sends the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void Send(Key key)
    {
        if (Keyboard.PrimaryDevice != null)
        {
            if (Keyboard.PrimaryDevice.ActiveSource != null)
            {
                var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                };
                InputManager.Current.ProcessInput(e);

                // Note: Based on your requirements you may also need to fire events for:
                // RoutedEvent = Keyboard.PreviewKeyDownEvent
                // RoutedEvent = Keyboard.KeyUpEvent
                // RoutedEvent = Keyboard.PreviewKeyUpEvent
            }
        }
    }
    private object func(Process process)
    {
        return process;
    }


    private void TcLink_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var url = Clipboard.GetText();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            if (VerifiedUrl(url)) ViewModel.SelectedProject.Details.TcHttpUrl = url;
        }
        else
        {
            if (url != "<DEFAULT>")
                ProcessStartHelper.OpenUrl(ViewModel.SelectedProject.Details.TcHttpUrl);
        }
    }

    private void GitLink_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var url = Clipboard.GetText();
        if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            if (VerifiedUrl(url)) ViewModel.SelectedProject.Details.GitHttpUrl = url;
        }
        else if (url != "<DEFAULT>")
        {
            ProcessStartHelper.OpenUrl(ViewModel.SelectedProject.Details.GitHttpUrl);
        }
    }

    private bool VerifiedUrl(string url)
    {
        return url.Contains("://");
    }

    private void Button_Save_Settings(object sender, RoutedEventArgs e)
    {
        //        var projectMenuNode = ViewModel.Projects.Node;
        //        var flat = new NodeFlattner().Flatten(projectMenuNode).ToList();
        //
        //        _cfg.Projects.ForEach(po =>
        //        {
        //            var realInstance = flat.SingleOrDefault(m => m.Id == po.Id);
        //            if (realInstance != null)
        //            {
        //                if (realInstance.TcHttpUrl != "<DEFAULT>")
        //                    po.TcHttpUrl = realInstance.TcHttpUrl;
        //                else
        //                    ;
        //            }
        //        });
        //        _appConfigPersistence.SaveConfig(_cfg);
        //new AppConfigPersistence().SaveConfig();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var httpUrl = ViewModel.SelectedProject.Details.GitHttpUrl.Substring(0,
            ViewModel.SelectedProject.Details.GitHttpUrl.Length - 4);
        var gitHttpUrl = httpUrl + "/-/merge_requests";
        ProcessStartHelper.OpenUrl(gitHttpUrl);
    }

    private void JiraNumberClick(object sender, MouseButtonEventArgs e)
    {
        if (CtrlIsPressed)
        {
            ViewModel.OpenSolutionForJira(ViewModel.JiraData.CurrentTicketName);
        }
        else
            ProcessStarter.OpenJira(ViewModel.JiraData.CurrentTicketName);
    }

    public bool CtrlIsPressed => (Keyboard.Modifiers & ModifierKeys.Control) != 0;

    private void Button_Click_7(object sender, RoutedEventArgs e)
    {
        ProcessStartHelper.OpenFolder(ViewModel.SelectedProject.FullPath);
    }

    private void Button_Click_8(object sender, RoutedEventArgs e)
    {
        //[DWS-2794] added ListConfigRules
        ProcessStarter.GitCommitProject(ViewModel.SelectedProject.FullPath,
            $"[{ViewModel.JiraData.CurrentTicketName}] ");
    }

    private void Button_Click_9(object sender, RoutedEventArgs e)
    {
        ProcessStarter.GitShowLogForProject(ViewModel.SelectedProject.FullPath);

    }

    private void Button_Refresh_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.RecalculateProjectGitStatussesAsync(new CancellationTokenSource().Token);
    }

    private void Button_RedeployThisVersionOfTheApp_Click(object sender, RoutedEventArgs e)
    {
        DeployHelper.Deploy();
    }

    private void Button_OpenAndEditAppInVisualStudio_OnClick(object sender, RoutedEventArgs e)
    {
        ProcessStartHelper.OpenRun(@"C:\Work\GitLabSync\GitLabSync.sln");
    }

    private void Button_Click_Folder(object sender, RoutedEventArgs e)
    {
        var Button_Click_Folder = Settings.Button_Click_Folder;
        ProcessStartHelper.OpenFolder(Button_Click_Folder);
    }

    private void Button_Click_Fork(object sender, RoutedEventArgs e)
    {
        ProcessStarter.OpenFork();
    }

    private void HelpClicked(object sender, MouseButtonEventArgs e)
    {
        _helpForMainWindow.Show();
    }

    //public event Action<object, object> Switch;

    private void Button_Click_BIGIP(object sender, RoutedEventArgs e)
    {
        /*private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
           ViewModel.SwitchToPreviousWindow();
           */

        ViewModel.SwitchToPreviousWindow();

        int delay = 50; // delay in milliseconds
        var sim = new InputSimulator();

        Thread.Sleep(delay);
        sim.Keyboard.TextEntry(Settings.AccountPassword);

        // Simulate pressing TAB
        Thread.Sleep(delay);
        sim.Keyboard.KeyPress(VirtualKeyCode.TAB);

        // Simulate pressing space
        Thread.Sleep(delay);
        sim.Keyboard.KeyPress(VirtualKeyCode.SPACE);

        // Simulate pressing enter
        Thread.Sleep(delay);
        sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
    }
}