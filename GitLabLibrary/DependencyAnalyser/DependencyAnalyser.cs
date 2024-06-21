using NucleusDependencyAnalyser;

using Newtonsoft.Json;

using NucleusDependencyAnalyserClass;

namespace NucleusDependencyAnalyserClass
{
    public class DependencyAnalyser
    {
        public void Run(string rootFolder)
        {
            //var projects = new ProjLocator(rootFolder).FindCsProjs();
            //var ptxt= JsonConvert.SerializeObject(projects);

            var result = JsonConvert.DeserializeObject<List<string>>(new hardcoded().ProjectsJson);

            var csfilesForAnalysis = result
                .Where(m => !m.Contains("Testing"))
                .Where(m => !m.Contains("UnitTests"))
                .Where(m => !m.Contains("Tests"))
                .ToArray();

            var contextBuilder = new ContextBuilder();

            var fileContexts = csfilesForAnalysis
                .Select(filepath => contextBuilder.Build(filepath))
                .Where(m=> m.Packages != null)
                .ToArray();

            var allUnique = fileContexts
                .SelectMany(m => m.Packages).Distinct().ToArray();

            var allNonNucleusUnique = allUnique
                //.Order()
                .ToArray();

            var output = string.Join("\n", allNonNucleusUnique);
        }
    }
}
