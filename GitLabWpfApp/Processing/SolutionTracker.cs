

using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace GitLabWpfApp.Processing;

public interface ISolutionTracker
{
    bool HasSolution(string slnCs);
    void BringUp(string slnCs);
    void Start(string sln);
}

public class SolutionTracker : ISolutionTracker
{
    private readonly string _procidsJson = "ProcIds.json";

    //static Dictionary<string, int> processesBySolution = new();

    private Dictionary<string, int> GetProccesses()
    {
        var procids = File.Exists(_procidsJson)
            ? JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(_procidsJson))
            : new Dictionary<string, int>();
        /*var newdic = new Dictionary<string, int>();
        foreach (var p in procids)
        {
            newdic.Add(p.Key.ToLower(), p.Value);
        }

        StoreProcIds(newdic);
        return newdic;*/

        return procids;
    }

    private void StoreProcIds(Dictionary<string, int> processesBySolution)
    {
        File.WriteAllText(_procidsJson,
            JsonConvert.SerializeObject(processesBySolution));
    }

    public bool HasSolution(string slnCs)
    {
        var sln = slnCs.ToLower();
        var processesBySolution = GetProccesses();

        if (processesBySolution.ContainsKey(sln))
        {
            var pid = processesBySolution[sln];

            try
            {
                var processById = Process.GetProcessById(pid);
                if (processById == null)
                {
                    processesBySolution.Remove(sln);
                    StoreProcIds(processesBySolution);
                    return false;
                }

                return true;
                ;
            }
            catch
            {
                processesBySolution.Remove(sln);
                StoreProcIds(processesBySolution);
                return false;
            }
        }

        return false;
    }

    public void BringUp(string slnCs)
    {
        var sln = slnCs.ToLower();
        var processesBySolution = GetProccesses();
        var pid = processesBySolution[sln];
        var processById = Process.GetProcessById(pid);
        if (!processById.HasExited)
        {
            var processMainWindowHandle = processById.MainWindowHandle;
            if (processMainWindowHandle != IntPtr.Zero) // was : nint.zero
                ForeGroundSetter.SetForegroundWindow(processMainWindowHandle);
        }
    }

    public void Start(string sln)
    {
        var process =
            Process.Start(
                @"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe",
                sln);
        var processesBySolution = GetProccesses();
        processesBySolution.Add(sln.ToLower(), process.Id);
        StoreProcIds(processesBySolution);
    }
}