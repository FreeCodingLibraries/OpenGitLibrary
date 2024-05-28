using GitLabLibrary._UserSpecific;

using LibGit2Sharp;
using Microsoft.Alm.Authentication;

namespace GitLabLibrary.Git;

public static class GitHelper
{
    public static FetchOptions GetFetchOptionsWithCredentials(Credential creds)
    {
        return new FetchOptions
        {
            CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
            {
                Username = creds.Username, Password = creds.Password
            }
        };
    }
}

public static class GitCredentialProvider
{
    public static Credential GetCredentials()
    {
        var secrets = new SecretStore("git");
        var auth = new BasicAuthentication(secrets);
        var creds = auth.GetCredentials(new TargetUri(Settings.GetCredentialsTargetUri));
        return creds;
    }
}