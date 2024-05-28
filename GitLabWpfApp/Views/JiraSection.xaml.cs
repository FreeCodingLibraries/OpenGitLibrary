using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GitLabLibrary._UserSpecific;

using GitLabWpfApp.Processing;
using GitLabWpfApp.Services.Jira.Models;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Views;

/// <summary>
///     Interaction logic for JiraSection.xaml
/// </summary>
public partial class JiraSection : UserControl
{
    public JiraSection()
    {
        InitializeComponent();
        DataContextChanged += (sender, args) =>
        {
            if (ViewModel == null)
                return;
            ViewModel.PropertyChanged += (o, eventArgs) =>
            {
                if (eventArgs.PropertyName == "OnSelectedTicketNameChanged")
                    ;

            };
        };
    }

    private JiraDataViewModel ViewModel => DataContext as JiraDataViewModel;

    private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
    {
        JiraDataViewModel viewModelJiraData = ViewModel;
        if (e.Key == Key.Escape)
            JiraNumber.Text = "";

        if (e.Key == Key.Enter)
        {
            var jiraNumberText = JiraNumber.Text;
            if (!jiraNumberText.Contains("-"))
                jiraNumberText = $"DWS-{jiraNumberText}";

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                AddJiraNumber(jiraNumberText);
            else
                ProcessStarter.OpenJira(jiraNumberText.Trim());
        }
    }

    private void Button_OpenJiraBoard(object sender, RoutedEventArgs e)
    {
        
        ProcessStartHelper.OpenUrl(Settings.JiraBoardUrl);
        //ProcessStartHelper.OpenUrl("https://nucleusjira.atlassian.net/jira/software/c/projects/DWS/boards/24");
    }


    private void JiraNumber_GotFocus(object sender, RoutedEventArgs e)
    {
        if (JiraNumber.Text == "JIRA")
            JiraNumber.Text = "";
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        AddJiraNumber(JiraNumber.Text);
    }

    private void AddJiraNumber(string jiraNumberText)
    {
        var jiraTicketModelObservable = new JiraDataViewModel.JiraTicketModelObservable()
        {
            StatusHistory = new ObservableCollection<JiraDataViewModel.JiraStatusChangedEventObservable>(),
            JiraName= jiraNumberText,
            Notes = "",
            RelatedProjects = new ObservableCollection<string>()    ,
            Status = JiraStatus.Available
        };
        ViewModel?.Tickets.Insert(0,jiraTicketModelObservable);
    }
    
    private void UIElement_OnKeyDown2(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                var selectedIndex = (sender as ListView)?.SelectedIndex;
                if (selectedIndex != null && selectedIndex > -1)
                {
                    ViewModel?.Tickets.RemoveAt(selectedIndex.Value);
                }
            }
        }
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var selectedIndex = (sender as ListView)?.SelectedIndex;
        if (selectedIndex != null && selectedIndex > -1)
        {
            var jira = ViewModel?.Tickets[selectedIndex.Value];
            ViewModel.CurrentTicketName = jira.JiraName;
        }

    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedIndex = JiraNumberListBox.SelectedIndex;
        if (selectedIndex == -1)
            return;

        var selectedItem = JiraNumberListBox.Items[selectedIndex];

        JiraNumber.Text = ((JiraDataViewModel.JiraTicketModelObservable)selectedItem).JiraName;
        ViewModel.SelectedTicketName  = JiraNumber.Text;
    }

    private void Button_ViewJiraByName_Click(object sender, RoutedEventArgs e)
    {
            var jiraNumberText = JiraNumber.Text;
            if (!jiraNumberText.Contains("-"))
                jiraNumberText = $"DWS-{jiraNumberText}";
        ProcessStarter.OpenJira(jiraNumberText.Trim());
    }

    private void Button_MyJiras_OnClick(object sender, RoutedEventArgs e)
    {
        ProcessStartHelper.OpenUrl(Settings.JiraBoardMyWorkUrl);
    }
}