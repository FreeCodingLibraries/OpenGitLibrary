using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Views
{
    public partial class WorkRelatedDetailsView : UserControl
    {
        public WorkRelatedDetailsView()
        {
            InitializeComponent();
        }

        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        private WorkRelatedDetailsViewModel ThisViewModel => ViewModel.WorkRelatedDetails;

        private void LoginButtonCliced(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchToPreviousWindow();
            ThisViewModel.SimulateLogin();
        }

        private void ClickUsername(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(ThisViewModel.YourUsername);
        }

        private void ClickDomain(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(ThisViewModel.YourDomain);
        }

        private void ClickHostName(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(ThisViewModel.YourHostName);
        }

        private void ClickYourEmail(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(ThisViewModel.YourEmail);
        }
    }
}