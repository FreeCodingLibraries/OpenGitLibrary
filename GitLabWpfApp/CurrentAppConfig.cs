using System.IO;

using GitLabLibrary._UserSpecific;

namespace GitLabWpfApp;

internal static class CurrentAppConfig
{
    public static string CodebaseLocalRootPath { get; set; } = Settings.RootCodeBaseFolder;
    public static string GitExtensionsExePath { get; set; } = @"C:\Program Files (x86)\GitExtensions\GitExtensions.exe";

    public static string NotepadPlusplusExePath { get; set; } =
        Path.Join(@"C:\Program Files\Notepad++", "notepad++.exe");

    public static string VsCodeExePath { get; set; } = Settings.vscodeExecPath;

    public static string VisualStudioExePath { get; set; } =
        @"C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe";
    //public static string  { get; set; }
    //public static string  { get; set; }
    //public static string  { get; set; }
}