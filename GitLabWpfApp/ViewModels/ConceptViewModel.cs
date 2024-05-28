using CommunityToolkit.Mvvm.ComponentModel;

namespace GitLabWpfApp.ViewModels;

public partial class ConceptViewModel : ObservableObject
{
    [ObservableProperty] private string _defaultBranch;
    [ObservableProperty] private string _gitHttpUrl;
    [ObservableProperty] private Guid? _id;

    [ObservableProperty] private string _name;
    [ObservableProperty] private string _nameWithNs;
    [ObservableProperty] private string _pathWithNs;

    [ObservableProperty] private string _tcHttpUrl;
    [ObservableProperty] private bool _visible;
    [ObservableProperty] private bool _correctBranch;
}