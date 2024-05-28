using System.Windows;

namespace GitLabWpfApp.SwitchAppHookup;

public interface IAppHookupStuffBuilder
{
    AppHookupStuffBuilder ExcludeThreadByWindow(Window window);
    AppHookupStuff Build();
}