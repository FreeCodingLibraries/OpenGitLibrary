using System.Net;
using System.Text;

using GitLabLibrary._UserSpecific;
using GitLabLibrary.Git;

using Microsoft.Alm.Authentication;

using RestSharp;
using RestSharp.Authenticators;

namespace GitLabLibrary.RepoGrouping
{
    public class GitLabProjectsManager : IGitLabRepoManager
    {
        public List<GitLabProjectContext> FetchGitLabProjects()
        {
            var url = Settings.FetchGitLabProjectsUrl;
            
            Credential credentials = GitCredentialProvider.GetCredentials();
            string? username = credentials.Username;
            string? password = credentials.Password;

            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes(username + ":" + password));

            WebProxy proxy = GetWebProxy();

            //////////////////// 1

//        var response = Get2WebResponseViaWebClient(username, password, proxy, url);


            //////////////////// 3

//        var response3 = GetResponseViaRestClient(url, username, password);

            //////////////////// 3

            RestResponse response4 = Get2WebResponseViaWebClient2(username, password, proxy, url);
            string? Tex3t = response4.Content; //404 group not found ,  if both pw is correct or wrong. 
            ;


            return null;
        }

        private static string Get2WebResponseViaWebClient(string username, string password, WebProxy proxy, string url)
        {
            WebClient client = new WebClient
            {
                Credentials = new NetworkCredential(username, password), Proxy = proxy // fixes 404 .. then get 403
            };
            client.Proxy = proxy;
            return client.DownloadString(url);
        }

        private static RestResponse Get2WebResponseViaWebClient2(string username, string password, WebProxy proxy,
            string url)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy // 404 not found , without a proxy setup
            };
            RestClientOptions opt = new RestClientOptions
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
            using (RestClient client = new RestClient(httpClientHandler))
            {
                RestRequest request = new RestRequest(url);
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(username + ":" + password));

                //  request.AddHeader("Authorization", $"Basic {encoded}");
                RestResponse response = client.Execute(request);
                return response;
            }

            ;
        }


        private static WebProxy GetWebProxy()
        {
            string MyProxyHostString = "http://some.proxy.server";
            int MyProxyPort = 80;
            WebProxy proxy = new WebProxy(MyProxyHostString, MyProxyPort);
            return proxy;
        }

        public string GetResponseViaRestClient(string url, string username, string password)
        {
            string encoded =
                Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));

            string MyProxyHostString =  Settings.GetWebProxyMyProxyHostString;
            int MyProxyPort =  Settings.GetWebProxyMyProxyPort;
            WebProxy proxy = new WebProxy(MyProxyHostString, MyProxyPort);
            RestClientOptions options = new RestClientOptions
            {
                Proxy = proxy, ThrowOnAnyError = true, MaxTimeout = 1000
            };
            RestClient client = new RestClient(options);

            RestRequest request = new RestRequest(url);

            //  request.AddHeader("Authorization", $"Basic {encoded}");
            RestResponse response = client.Execute(request);
            return response.Content; //404 group not found ,  if both pw is correct or wrong. 
        }

        private CredentialCache MakeCachedCredential(string url, string username, string password)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
            return credentialCache;
        }
    }
}