using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GitLabWpfApp.Tnodes;

namespace GitLabWpfApp.Components.ProjectTree;

internal class HeaderMaker
{
    public static MenuItem MakeHeader(TNode node)
    {
        var header = new MenuItem();
        header.Tag = node;
        header.Header = node.PathName;
        header.Background = new SolidColorBrush(Colors.CornflowerBlue);
        header.Foreground = new SolidColorBrush(Colors.White);
        header.FontWeight = FontWeights.Bold;
        header.IsEnabled = false;
        return header;
    }
}