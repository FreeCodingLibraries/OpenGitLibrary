using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitLabWpfApp.Views
{
    /// <summary>
    /// Interaction logic for CodeTemplates.xaml
    /// </summary>
    public partial class CodeTemplates : UserControl
    {
        public CodeTemplates()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SendText(@"    [TestFixture]
    public class Tests
    {
        public void Test()
        {
            // Arrange
            var _sut = new object();

            // Act
            var result = _sut.ToString();

            // Assert
            result.ShouldBe("""");
        }
    }
");
        }

        private void SendText(string txt)
        {
         //   //Install - Package InputSimulator
         //   InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
         //
         //   //var keyboardSimulator = new InputSimulator().Keyboard;
         //   new InputSimulator().Keyboard..SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
         //
         //   //keyboardSimulator.ModifiedKeyStroke(new ModifierKeys[]{ ModifierKeys.Alt},new VirtualKeyCode[]{ VirtualKeyCode.TAB});
         //   keyboardSimulator.KeyDown( Alt);
         //   keyboardSimulator.KeyDown(VirtualKeyCode.TAB);
         //   keyboardSimulator.TextEntry("hello");
        }
    }
}
