namespace GitLabLibrary.RepoGrouping;

public class GitLabProjectContext
{
    public int id { get; set; }
    public string description { get; set; }
    public string name { get; set; }
    public string name_with_namespace { get; set; }
    public string path { get; set; }
    public string path_with_namespace { get; set; }
    public DateTime created_at { get; set; }
    public string default_branch { get; set; }
    public object[] tag_list { get; set; }
    public object[] topics { get; set; }
    public string ssh_url_to_repo { get; set; }
    public string http_url_to_repo { get; set; }
    public string web_url { get; set; }
    public string readme_url { get; set; }
    public string avatar_url { get; set; }
    public int forks_count { get; set; }
    public int star_count { get; set; }
    public DateTime last_activity_at { get; set; }
    public Namespace _namespace { get; set; }

    public class Namespace
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string kind { get; set; }
        public string full_path { get; set; }
        public int parent_id { get; set; }
        public string avatar_url { get; set; }
        public string web_url { get; set; }
    }
}