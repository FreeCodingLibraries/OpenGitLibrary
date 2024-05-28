using GitLabWpfApp.SwitchAppHookup;

namespace GitLabWpfApp
{
    internal class WindowFindDemo
    {
        public void GoTo()
        {
            string className = "Notepad"; // Example class name
            string windowTitle = "Untitled - Notepad"; // Example window title

            IntPtr hWnd = DllFuncs.FindWindowByClassNameAndTitle(className, windowTitle);

            if (hWnd != IntPtr.Zero)
            {
                bool focused = DllFuncs.SetFocusToWindow(hWnd);
                Console.WriteLine(focused ? "Window focused successfully." : "Failed to focus window.");
            }
            else
            {
                Console.WriteLine("Window not found.");
            }

            Environment.Exit(0);
        }
    }
}