using CommunityToolkit.Mvvm.ComponentModel;

using GitLabLibrary._UserSpecific;

using WindowsInput.Native;
using WindowsInput;

namespace GitLabWpfApp.ViewModels
{
    public partial class WorkRelatedDetailsViewModel : ObservableObject
    {
        [ObservableProperty] private string _yourUsername = Environment.UserName;
        [ObservableProperty] private string _yourDomain = Environment.UserDomainName;
        [ObservableProperty] private string _yourHostName = Environment.MachineName;
        [ObservableProperty] private string _yourEmail = Settings.YourEmail;
        [ObservableProperty] private string _jobStartDate = Settings.JobStartDate;
        [ObservableProperty] private string _yourProxyServerAndPort = Settings.YourProxyServerAndPort;
        [ObservableProperty] private string _yourCostCentre = Settings.YourCostCentre;
        [ObservableProperty] private string _yourAddress = Settings.YourAddress;
        [ObservableProperty] private string _yourLineManager = Settings.YourLineManager;
        
        public void SimulateLogin()
        {
            string password = "password";

            int delay = 50; // delay in milliseconds
            var sim = new InputSimulator();

//           // Press and hold the ALT key
//           Thread.Sleep(delay);
//           sim.Keyboard.KeyDown(VirtualKeyCode.MENU);
//
//           // Press the TAB key
//           Thread.Sleep(delay);
//           sim.Keyboard.KeyPress(VirtualKeyCode.TAB);
//
//           // Release the ALT key
//           Thread.Sleep(delay);
//           sim.Keyboard.KeyUp(VirtualKeyCode.MENU);

            // Simulate pressing a key
          //  Thread.Sleep(delay);
          //  sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            Thread.Sleep(delay);

            sim.Keyboard.TextEntry(YourUsername);

            // Simulate pressing TAB
            Thread.Sleep(delay);
            sim.Keyboard.KeyPress(VirtualKeyCode.TAB);

            Thread.Sleep(delay);
            sim.Keyboard.TextEntry(YourUsername);

            // Simulate pressing TAB
            Thread.Sleep(delay);
            sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            //  foreach (char c in text)
            //  {
            //      sim.Keyboard.TextEntry(c);
            //      Thread.Sleep(delay);
            //  }

            // Simulate pressing a key
            // sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
    }
}

