using System.Diagnostics;
using System.Windows.Controls;
using GitLabWpfApp.Tnodes;
using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp.Components.ProjectTree;

internal class TeamCityNodeMaker
{
    public static MenuItem MakeTC(TNode node)
    {
        var teamcityBuildNode = new MenuItem();
        teamcityBuildNode.Tag = node;

        teamcityBuildNode.Header = "TeamCity build";
        teamcityBuildNode.Click += (e, a) =>
        {
            var pC = node.Projects.FirstOrDefault();
            var finallink = CreateTcLink(pC);
            if (finallink == null)
                return;
            var link =
                @$"https://someTcDomain/buildConfiguration/{finallink.MainPart}_{finallink.BuildFolder}";
            //var final = (link.Contains("?")) ? link : link + "_NetBuildTestAndPublish?mode=builds#all-projects";
            var final = link + "?mode=builds#all-projects";
            //https://someTcDomain/buildConfiguration/projname_Domains_other_otherClientApi_NetBuildTestAndPublish?mode=builds#all-projects

            Process.Start("explorer.exe", $"\"{final}\"");
        };

        return teamcityBuildNode;
    }

    private static TeamCityLinkParts CreateTcLink(ConceptViewModel pC)
    {
        var nm = pC.PathWithNs;
        var prefixLen = "rootcodepath/".Length;
        var url = nm.Substring(prefixLen);
        switch (url)
        {
            case "projname/domains/other/other-service":
                return new TeamCityLinkParts("projname_Domains_other_other", "NetBuildTestAndPublish");
                break;
            default:
                return null; // throw new Exception("not setup");
        }
    }
}