using Newtonsoft.Json;

namespace GitLabLibrary;

public class RawConceptItem
{
    [JsonIgnore] public Guid? Id { get; set; }

    public string Name { get; set; }
    public string GitHttpUrl { get; set; }
    public string TcHttpUrl { get; set; }
    public string DefaultBranch { get; set; }
    public string NameWithNs { get; set; }
    public string PathWithNs { get; set; }
    public bool Visible { get; set; }
}