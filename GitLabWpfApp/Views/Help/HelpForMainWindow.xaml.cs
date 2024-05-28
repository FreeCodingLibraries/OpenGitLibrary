using System.Windows;
using System.Windows.Input;

namespace GitLabWpfApp.Views.Help;

public partial class HelpForMainWindow : Window
{
    public HelpForMainWindow()
    {
        InitializeComponent();
        Deactivated += (sender, args) => { Hide(); };
        KeyDown += (sender, args) =>
        {
            if (args.Key == Key.Escape)
                Hide();
        };
    }
}