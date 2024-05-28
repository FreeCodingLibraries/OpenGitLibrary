using System.IO;

using GitLabWpfApp.Framework.Logging;
using GitLabWpfApp.Services.Jira.Models;
using Newtonsoft.Json;

using static System.Net.Mime.MediaTypeNames;

namespace GitLabWpfApp.Services.Jira;

public class JiraPersistence : IJiraPersistence
{
    private readonly string _filePath;
    private readonly ILogger _logger;

    public JiraPersistence(string filePath, ILogger logger)
    {
        _filePath = filePath;
        _logger = logger;
    }

    public JiraDataDto LoadJiraData()
    {
        if (File.Exists(_filePath))
        {
            var txt = File.ReadAllText(_filePath);
            var model = JsonConvert.DeserializeObject<JiraDataDto>(txt);
            _logger.Log($"Loading Jira model:\n{txt}");
            return model;
        }
        _logger.Log($"Loading Jira model:\n(null)");

        return new();
    }

    public void SaveJiraData(JiraDataDto model)
    {

        var txt = JsonConvert.SerializeObject(model, Formatting.Indented);
        _logger.Log($"Saving new model:\n{txt}");
        File.WriteAllText(_filePath , txt);
    }
}