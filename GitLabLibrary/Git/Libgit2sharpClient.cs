using GitLabLibrary._UserSpecific;
using GitLabLibrary.Git.Results;
using LibGit2Sharp;

namespace GitLabLibrary.Git;

public class Libgit2sharpClient : IGitClient
{
    public GitCloneResult Clone(string repoUrl, string targetPath, string branch, string origin = "origin")
    {
        try
        {
            var creds = GitCredentialProvider.GetCredentials();
            var fetchOptions = GitHelper.GetFetchOptionsWithCredentials(creds);
            var options = new CloneOptions
            {
                BranchName = branch,
                CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = creds.Username,
                    Password = creds.Password
                }
            };

            Repository.Clone(repoUrl, targetPath, options);
            return new GitCloneResult(true);
        }
        catch (Exception e)
        {
            return new GitCloneResult(false);
        }
    }

    public GitPullResult Pull(string targetPath, string branch, string origin = "origin")
    {
        try
        {
            var creds = GitCredentialProvider.GetCredentials();
            var fetchOptions = GitHelper.GetFetchOptionsWithCredentials(creds);

            var refspecs = new string[] { };

            using (var repo = new Repository(targetPath))
            {
                var remoteName = repo.Network.Remotes[origin].Name;
                repo.Network.Fetch(remoteName, refspecs, fetchOptions);
                var remoteBranch = repo.Branches[$"origin/{branch}"];

                var headFriendlyName = repo.Head.FriendlyName; //currently checked out branch name
                var curcheckedoutBranch = repo.Branches[headFriendlyName]; //currently checked out branch entity
                var trackinBranch =
                    curcheckedoutBranch
                        .TrackedBranch; //currently checked out branch , remote tracking branch (if tracked, otherwise null)

                if (headFriendlyName != branch)
                    ; // you have another branch currently checked out , than the default (master or main etc) branch.

                if (trackinBranch != null)
                {
                    var requireMerge = trackinBranch.Tip.Sha != curcheckedoutBranch.Tip.Sha;
                    if (requireMerge)
                    {
                        var mergeResult = repo.Merge(trackinBranch,
                            //SignatureName, SignatureEmail
                            new Signature(Settings.SignatureName, Settings.SignatureEmail, DateTimeOffset.Now),
                            new MergeOptions());
                        var mm = mergeResult.Status switch
                        {
                            MergeStatus.FastForward => MyMergeStatus.FF,
                            MergeStatus.Conflicts => MyMergeStatus.Conflict,
                            MergeStatus.NonFastForward => MyMergeStatus.NonFFwd,
                            MergeStatus.UpToDate => MyMergeStatus.UptoDate,
                            _ => throw new Exception("not found")
                        };

                        if (mm == MyMergeStatus.Conflict)
                            return new GitPullResult(false, requireMerge, mm);

                        return new GitPullResult(true, requireMerge, mm);
                    }

                    return new GitPullResult(true, requireMerge, MyMergeStatus.UptoDate);
                }

                Console.WriteLine($"        local branch '{curcheckedoutBranch}' is not tracked remotely");
                return new GitPullResult(false, false, failure: "local branch is not tracked remotely");

                /*var areEqual = remoteBranch.Tip.Sha == remoteBranch.Tip.Sha;
                if (areEqual)
                    return new GitPullResult(true, MyMergeStatus.FF);
                return new GitPullResult(false);*/

                //var localbranch = repo.Branches[headFriendlyName];
                //var localMasterBranch = repo.Branches[_master_];
                //       if (localMasterBranch.Tip.Sha != remoteMasterBranch.Tip.Sha)
                //           ;//outdated master

                //var trackingbranch = localbranch.TrackedBranch;
                //if (trackingbranmnull)
                //    ;
            }
        }
        catch (Exception e)
        {
            return new GitPullResult(false, false, failure: e.Message);
        }
    }


    public bool Fetch(string targetPath, string origin = "origin")
    {
        try
        {
            var creds = GitCredentialProvider.GetCredentials ();
            var fetchOptions = GitHelper.GetFetchOptionsWithCredentials(creds);

            var remoteName = origin;
            var refspecs = new string[] { };

            using (var repo = new Repository(targetPath))
            {
                repo.Network.Fetch(remoteName, refspecs, fetchOptions);
                return true;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public string GetBranchName(string targetPath)
    {
        using (var repo = new Repository(targetPath))
        {
            var repoHead = repo.Head;
            var friendly = repoHead?.FriendlyName;
            var tracked = repoHead.TrackedBranch?.FriendlyName;

            return tracked?.Substring(7) ?? friendly;
        }
    }
}