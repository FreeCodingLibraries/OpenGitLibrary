using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

using GitLabWpfApp.Extensions;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Views;

/// <summary>
///     Interaction logic for ProjectsMenu.xaml
/// </summary>
public partial class ProjectsMenu : UserControl
{
    public ProjectsMenu()
    {
        InitializeComponent();
    }

    public MainWindowViewModel MainViewModel => DataContext as MainWindowViewModel;

    private void TreeView1_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        MainViewModel.SelectedProject = SelectedTreeItem;
    }

    private ProjectNodeViewModel? SelectedTreeItem => treeView1.SelectedItem as ProjectNodeViewModel;

    private void DblClickedOnProject(object sender, MouseButtonEventArgs e)
    {
        if (SelectedTreeItem != null)
        {
            MainViewModel.DblClickOnProject(SelectedTreeItem);

            //    var path = Path.Join(node.LocalFolderPath, node.PathName);
            //    var arg = "\"" + path + "\"";
            //    //var arg = "/select, \"" + path + "\"";
            //    Process.Start("explorer.exe", arg);



            // SelectedTreeItem.FullPath
            // var path = Path.Join(SelectedTreeItem.Details..LocalFolderPath, node.PathName);
            //var arg = "\"" + path + "\"";
            //var arg = "/select, \"" + path + "\"";
            //Process.Start("explorer.exe", arg);
        }
    }

    private void TreeView1_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var isCtrlPressed = ((Keyboard.Modifiers & ModifierKeys.Control) != 0);
        var nodeVm = (sender as FrameworkElement)?.DataContext as ProjectNodeViewModel;
        if (nodeVm != null)
        {
            if (isCtrlPressed)
            {
                nodeVm.IsSelected = true;
            }
            else
            {
                nodeVm.IsSelected = false;

            }
        }
    }



    private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
    {
        ;
    }

    private void SourceTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _startPoint = e.GetPosition(null);
    }
    private Point _startPoint;
    private void SourceTreeView_MouseMove(object sender, MouseEventArgs e)
    {
        Point mousePos = e.GetPosition(null);
        Vector diff = _startPoint - mousePos;

        if (e.LeftButton == MouseButtonState.Pressed &&
            (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
             Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        {
            TreeView treeView = sender as TreeView;
            TreeViewItem treeViewItem = FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

            if (treeViewItem != null)
            {
                // Initialize the drag & drop operation
                //var data = (treeViewItem.Header as ProjectNodeViewModel).DisplayName;
                var node = (treeViewItem.Header as ProjectNodeViewModel);
                var data = GetFlatNodes(node).Select(m=>m.FullName).ToArray();
                DataObject dragData = new DataObject(DragDropDataFormat.ProjectName, data);
                DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Link);
            }
        }
    }

    private IEnumerable<ProjectNodeViewModel> GetFlatNodes(ProjectNodeViewModel node)
    {
        if (!node.Children.Any())
            yield return node;
        else
        {
            foreach (ProjectNodeViewModel nodi in node.Children)
            {
                IEnumerable<ProjectNodeViewModel> nodeViewModels = GetFlatNodes(nodi);
                foreach (ProjectNodeViewModel n in nodeViewModels)
                {
                    yield return n;
                }
            }
        }

        IList<ProjectNodeViewModel> projectNodeViewModels = node.Children.Flatten();

    }

    private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T)
            {
                return (T)current;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }
}