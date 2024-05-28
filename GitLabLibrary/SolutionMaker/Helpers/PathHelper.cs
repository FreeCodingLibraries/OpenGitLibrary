namespace GitLabLibrary.SolutionMaker.Helpers
{
    internal static class PathHelper
    {
        public static string PathDiff(string fromPath, string toPath)
        {
            // Ensure the paths end with a directory separator to correctly treat them as directories
            if (!fromPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                fromPath += Path.DirectorySeparatorChar;


            if (!toPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                toPath += Path.DirectorySeparatorChar;

            // Create Uri instances for both paths
            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            // Calculate the relative path from 'fromUri' to 'toUri'
            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            // Convert the relative Uri to a relative path string
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            // Replace the URL path separator '/' with the system's directory separator character
            relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
            return relativePath.TrimEnd(Path.DirectorySeparatorChar);

            //return relativePath;
        }
    }
}