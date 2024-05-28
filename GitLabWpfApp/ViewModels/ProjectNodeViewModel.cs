using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using GitLabWpfApp.Processing;
using GitLabWpfApp.Tnodes;
using Color = System.Windows.Media.Color;

namespace GitLabWpfApp.ViewModels;

public partial class ProjectNodeViewModel : ObservableObject
{
    [ObservableProperty] private bool _isDirty;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private ObservableCollection<ProjectNodeViewModel> _children = new();

   // private SolidColorBrush _fgColor;
    public SolidColorBrush FgColor
    {
        get
        {
            var black = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            var red = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            var green = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            var blue = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            var pink = new SolidColorBrush(Color.FromRgb(200, 20, 215));
            //var white = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            return IsInGitSyncWithJiraNumber switch
            {
                GitSyncStatus.updateing => pink,
                GitSyncStatus.master => blue,
                GitSyncStatus.nonRepo => black,
                GitSyncStatus.outOfSync => red,
                GitSyncStatus.synced => green,
                _ => throw new NotImplementedException()
            };
        }
        /*set
        {
            if (_fgColor != value)
            {
                _fgColor = value;
                OnPropertyChanged(nameof(FgColor));
            }
        }*/
    }

    [ObservableProperty] private ConceptViewModel _details;
    [ObservableProperty] private string _displayName;
    [ObservableProperty] private string _fullName;
    private ISolutionTracker _solutionTracker;

    public ProjectNodeViewModel(ISolutionTracker solutionTracker)
    {
        _solutionTracker = solutionTracker;

        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != nameof(IsDirty))
            {
                IsDirty = true;
            }
        };
    }
    [ObservableProperty]
    private bool _isExpanded = false;
   
    [ObservableProperty]
    private string  _fullPath ;

    [NotifyPropertyChangedFor(nameof(FgColor))]
    [ObservableProperty] private GitSyncStatus _isInGitSyncWithJiraNumber;
    
    public void ClearDirtFlag()
    {
        _isDirty = false;
    }

//    public void OnIsSelectedChanged()
//    {
//        MainWindowViewModel
//    }
}

public enum GitSyncStatus
{
    updateing = 0,
    master,
    synced,
    nonRepo,
    outOfSync
}