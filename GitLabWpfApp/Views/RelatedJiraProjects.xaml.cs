using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using GitLabLibrary.SimpleBus;

using GitLabWpfApp.Extensions;
using GitLabWpfApp.Processing;
using GitLabWpfApp.ViewModels;

using Microsoft.Extensions.DependencyInjection;

namespace GitLabWpfApp.Views
{
    /// <summary>
    /// Interaction logic for RelatedJiraProjects.xaml
    /// </summary>
    public partial class RelatedJiraProjects : UserControl
    {
        private MainWindowViewModel VmMain;
        private RelatedJiraProjectsViewModel Vm;
        private readonly IBusPublisher? _busPublisher;

                
        public string SelectedItemProjectFullname => TargetTreeView.SelectedItem as string;

        public RelatedJiraProjects()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                if (args.NewValue is MainWindowViewModel mainone)
                {
                    this.VmMain = mainone;
                    this.Vm = mainone?.JiraProjects;
                }
            };

            //Vm = App.Services.GetService<RelatedJiraProjectsDataContext>();
          //  _busPublisher = App.Services.GetService<IBusPublisher>();
          //  var subscriber = App.Services.GetService<IBusSubscriber>();
          //  subscriber.Subscribe("RelatedJiraProjects (UI)", OnMessageReceived);
          //  this.DataContext = Vm;
        }

        private void TargetTreeView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DragDropDataFormat.ProjectName))
            {
                //string itemHeader = e.Data.GetData(DragDropDataFormat.ProjectName) as string;
                var items = e.Data.GetData(DragDropDataFormat.ProjectName) as string[];
                          Vm?.OnProjectsDropped?.Invoke(this, items );
            }
        }

        private void TargetTreeView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var item = TargetTreeView.SelectedItem as string;
                if (item != null)
                {
                    Vm.Projects.Remove(item);
                }
                       Vm?.OnProjectsDeleted?.Invoke(this, null);
 }

        }

        private void TargetTreeView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedItemProjectFullname != null)
            {
                VmMain.OpenSingleProjectSolutionByFullName(SelectedItemProjectFullname);
            }
        }

        private void CreateSolution(object sender, RoutedEventArgs e)
        {
            var allProjs = VmMain.Projects.Flatten();
            var relatedProjVms = allProjs.Where(m => Vm.Projects.Contains(m.FullName));
            var relatedProjPaths=relatedProjVms.Select(m => m.FullPath);

            VmMain.CreateSolution(VmMain.JiraData.SelectedTicketName, relatedProjPaths);
        }

        private void OpenSolution(object sender, RoutedEventArgs e)
        {
            VmMain.OpenSolutionForJira(VmMain.JiraData.SelectedTicketName);
        }
    }
}
