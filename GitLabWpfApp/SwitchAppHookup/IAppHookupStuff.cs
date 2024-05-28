namespace GitLabWpfApp.SwitchAppHookup;

public interface IAppHookupStuff
{
    void Dispose();
    AppHookupStuff Setup();
    void SwitchToLastActiveApp();
}