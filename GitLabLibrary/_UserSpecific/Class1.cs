using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLabLibrary._UserSpecific
{
    public class Settings
    {
        public static string SrcProjAbsoluteFolder = @"c:\Work\tempbaseforsolutions";
        public static string BaretailExe = @"C:\Portable\Portable\baretail.exe";

        public static string FetchGitLabProjectsUrl = "https://my.gitlab/api/v4/groups/1234/projects?simple=true&archived=false&include_subgroups=true&per_page=100";
        public static string GetCredentialsTargetUri = "https://some.gitlab.repo";
        public static string FetchGitLabProjectsFolder = @"C:\Work\GitLabSync\GitLabLibrary\projects.json";
        public static string AppConfigPersistenceFolder = @"AppConfiguration.json"; //@"C:\Work\GitLabSync\AppConfiguration.json";
        public static string SignatureName = "My Name";
        public static string SignatureEmail = "my@email.com";
        public static string GetWebProxyMyProxyHostString = "http://some.relevant.proxy";
        public static int GetWebProxyMyProxyPort = 80;

        public static string DeployHelperFolder = @"C:\GitSyncDeployFolder";
        public static string RootCodeBaseFolder = @"c:\work\code";
        public static string vscodeExecPath = Path.Join(@"C:\Code.exe");
        public static bool UseFakeProjVmProvider = true;
        public static string JiraBoardUrl => "notset";
        public static string JiraBoardMyWorkUrl => "notset";

        //work related details
        public static string YourEmail = "pietman@gmail.com";
        public static string JobStartDate = "Mon 5 Feb 2024";
        public static string YourProxyServerAndPort = "http://proxy:80";
        public static string YourCostCentre = "http://proxy:80";
        public static string YourAddress = "23 KGave, KT12 3LP";
        public static string YourLineManager = "Mr X; comp no: 12345";

        public static string AccountPassword = @"abc123";
        public static string Button_Click_Folder = @"C:\Work\code\root-path-here\deployments\platform-deployment-swaps";
    }
}
