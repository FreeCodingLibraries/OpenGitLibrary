using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GitLabWpfApp.ViewModels;
using Path = System.IO.Path;

namespace GitLabWpfApp.Views;

/// <summary>
///     Interaction logic for SolutionSearchControl.xaml
/// </summary>
public partial class SolutionSearchControl : UserControl
{
    private static List<string> _enumerateFiles = new();
    private readonly string _notepadExe;
    private readonly string defaultEditor;

    public SolutionSearchControl()
    {
        _notepadExe = CurrentAppConfig.NotepadPlusplusExePath;
        var _codeExe = CurrentAppConfig.VsCodeExePath;
        defaultEditor = _codeExe;

        InitializeComponent();
        SearchTxtBox.Focus();
        var startSearch = false;
        if (startSearch)
        {Task.Run(() =>
        {
            if (!_enumerateFiles.Any())
            {
                var codebaseLocalRootPath =
                    Path.Combine(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here");
                _enumerateFiles = MyDirectory.EnumerateFiles(codebaseLocalRootPath, "*.cs", SearchOption.AllDirectories)
                    .ToList();
                Dispatcher.Invoke(() => { SearchTxtBox_OnTextChanged(this, null); });
            }
        });}
        //SearchTxtBox.Text = "negotiationreporteventhandler";
    }

    public SearchCtrlViewModel ViewModel => DataContext as SearchCtrlViewModel;


    private void SearchTxtBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var hasText = SearchTxtBox.Text.Length > 1;
        searchadvice.Visibility = hasText ? Visibility.Collapsed : Visibility.Visible;

        var setsOptions = SearchIncludesPath.AnywhereInPath;
        var options = SearchIncludesTestOptions.ExcludeAllTests;

        lbox.Items.Clear();

        var text = SearchTxtBox.Text.ToUpper().Trim();
        if (text == "")
            return;
        var mixedterms = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var terms = mixedterms.Where(m => !m.StartsWith("!"));
        var negativeTerms = mixedterms.Where(m => m.StartsWith("!")).Select(m => m.Substring(1))
            .Where(m => m.Length > 0);

        var join = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "");
        var offset = join.Length + 1;
        var filenames = _enumerateFiles.Where(fullfile =>
            {
                var subpathonly = fullfile.Substring(offset);
                var file = subpathonly;
                var filePathUpperCase = file.ToUpper();
                //var fnnameonly = ;
                var upper = Path.GetFileNameWithoutExtension(filePathUpperCase); //fnnameonly.ToUpper();
                var containsAllTerms =
                    setsOptions == SearchIncludesPath.OnlyInFilename
                        ? terms.All(x => upper.Contains(x))
                        : terms.All(x => filePathUpperCase.Contains(x));

                var containsNegative = negativeTerms.Any(m => upper.Contains(m));

                return containsAllTerms
                       && !containsNegative
                       && (
                           options == SearchIncludesTestOptions.NoDiscrimination
                           || (options == SearchIncludesTestOptions.IncludeOnlyTests &&
                               filePathUpperCase.Contains("TEST"))
                           || (options == SearchIncludesTestOptions.ExcludeAllTests &&
                               !filePathUpperCase.Contains("TEST"))
                       );
            })
            .ToList();

        foreach (var file in filenames)
        {
            var subpathonly = file.Substring(offset);
            lbox.Items.Add(subpathonly);
        }
    }

    private void lbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var fnIn = lbox.SelectedItems[0].ToString();
        var fn = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fnIn);

        //\\HOSTNAME\c$\Work\GitLabSync
        //   var pcpath = Path.Join(CurrentAppConfig.CodebaseLocalRootPath.Replace("c:\\", "\\\\HOSTNAME\\c$"), "root-path-here", fn);
        //   var path = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fn);
        var quickpath = fn;
        //   var lastpcpath = fn.Replace("c:\\", "\\\\HOSTNAME\\c$");

        var usepath = quickpath;
        var arg = "\"" + usepath + "\"";
        Process.Start(defaultEditor, arg);
    }

    private void Lbox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.IsDown && e.Key == Key.V)
        {
            var allSolutions = new List<string>();
            foreach (var opensubpath in lbox.SelectedItems)
            {
                var fn = opensubpath.ToString();
                var path = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fn);
                var folder = Path.GetDirectoryName(path);
                var sln = GetSln(folder);
                if (sln == null)
                    continue;

                allSolutions.Add(sln);
            }

            foreach (var solution in allSolutions.Distinct())
            {
                var arg = "\"" + solution + "\"";
                //Process.Start("explorer.exe", arg);
                Process.Start(CurrentAppConfig.VisualStudioExePath, arg);
                //       return;
            }
        }

        if (e.IsDown && e.Key == Key.Enter)
            foreach (var opensubpath in lbox.SelectedItems)
            {
                var fn = opensubpath.ToString(); //lbox.SelectedItems[0].ToString();
                var path = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fn);
                var arg = "\"" + path + "\"";
                Process.Start(defaultEditor, arg);
            }

        if (e.IsDown && e.Key == Key.N)
            foreach (var opensubpath in lbox.SelectedItems)
            {
                var fn = opensubpath.ToString(); //lbox.SelectedItems[0].ToString();
                var path = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fn);
                var arg = "\"" + path + "\"";
                Process.Start(_notepadExe, arg);
            }

        if (e.IsDown && e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
        {
            var sb = new StringBuilder();
            foreach (var opensubpath in lbox.SelectedItems)
            {
                var fn = opensubpath.ToString(); //lbox.SelectedItems[0].ToString();
                var path = Path.Join(CurrentAppConfig.CodebaseLocalRootPath, "root-path-here", fn);
                sb.AppendLine(path);
            }

            Clipboard.SetText(sb.ToString());
        }
    }

    private string? GetSln(string? folder)
    {
        if (folder == null)
            return null;
        var files = Directory.GetFiles(folder, "*.sln");
        if (files.Length > 0)
            return files[0];
        return GetSln(Path.GetDirectoryName(folder));
    }

    private void Lbox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
            return;

        if (lbox.SelectedItems.Count == 0) return;
        ViewModel.SelectedFilename = lbox.SelectedItems[0].ToString();
    }
}