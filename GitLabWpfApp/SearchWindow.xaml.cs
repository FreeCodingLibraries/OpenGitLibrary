using System.IO;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace GitLabWpfApp;

/// <summary>
///     Interaction logic for SearchWindow.xaml
/// </summary>
public partial class SearchWindow : Window
{
    private static List<string> _enumerateFiles = new();

    public SearchWindow()
    {
        if (!_enumerateFiles.Any())
            _enumerateFiles = Directory.EnumerateFiles(CurrentAppConfig.CodebaseLocalRootPath, "*.cs", SearchOption.AllDirectories)
                .ToList();
        InitializeComponent();
    }

    private void SearchTxtBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        lbox.Items.Clear();

        var options = SearchIncludesTestOptions.ExcludeAllTests;

        var text = SearchTxtBox.Text.ToUpper().Trim();
        var filenames = _enumerateFiles.Where(file =>
            {
                var fnnameonly = Path.GetFileNameWithoutExtension(file);
                var upper = fnnameonly.ToUpper();
                return upper.Contains(text)
                       && (
                           options == SearchIncludesTestOptions.NoDiscrimination
                           || (options == SearchIncludesTestOptions.IncludeOnlyTests && upper.Contains("TEST"))
                           || (options == SearchIncludesTestOptions.ExcludeAllTests && !upper.Contains("TEST"))
                       );
            })
            .ToList();
        foreach (var file in filenames) lbox.Items.Add(file);
    }

   
}