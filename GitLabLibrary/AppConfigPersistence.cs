
using GitLabLibrary._UserSpecific;

using Newtonsoft.Json;

namespace GitLabLibrary;

public class AppConfigPersistence
{
    
 
    private   string path => Settings.AppConfigPersistenceFolder;  

    public AppConfiguration LoadConfig()
    {
        if (!File.Exists(path))
            return new();

        var txt = File.ReadAllText(path);
        var oo = JsonConvert.DeserializeObject<AppConfiguration>(txt);
        foreach (var p in oo.Projects)
            if (p.Id == null)
                p.Id = Guid.NewGuid();
        return oo;
    }

    public void SaveConfig(AppConfiguration config)
    {
        var cntnt = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(path, cntnt);
    }
}