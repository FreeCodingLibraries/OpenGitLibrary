namespace GitLabWpfApp.Framework.Logging
{
    public interface ILogger
    {
        void OpenBaretail();

        void Log(string message);
    }
}
