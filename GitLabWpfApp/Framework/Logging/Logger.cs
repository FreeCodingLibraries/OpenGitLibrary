using System.Diagnostics;

using GitLabLibrary._UserSpecific;

using Serilog;

namespace GitLabWpfApp.Framework.Logging;

public class Logger : ILogger, IDisposable
{
    private readonly Serilog.ILogger _logger;
    private string _logTxt;

    public Logger()
    {
        _logTxt = "log.txt";
        _logger = new LoggerConfiguration()
            //.WriteTo.Console()
            .WriteTo.File(_logTxt)//, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Serilog.Log.Logger = _logger;
    }

    public void OpenBaretail()
    {
        var bt = Settings.BaretailExe;
        Process.Start(bt, _logTxt);
    }

    public void Log(string message)
    {
        _logger.Information(message);
    }

    public void Dispose()
    {
        Serilog.Log.CloseAndFlush();
    }
}