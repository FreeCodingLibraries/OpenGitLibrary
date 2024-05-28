using CommunityToolkit.Mvvm.ComponentModel;

namespace GitLabWpfApp.ViewModels;

public partial class SearchCtrlViewModel : ObservableObject
{
    [ObservableProperty] private string _selectedFilename = "YESYESYES";
}