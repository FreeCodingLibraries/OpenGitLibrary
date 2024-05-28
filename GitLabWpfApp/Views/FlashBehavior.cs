using System.Windows.Controls;
using System.Windows.Media.Animation;

using Microsoft.Xaml.Behaviors;

namespace GitLabWpfApp.Views
{
    public class FlashBehavior : Behavior<Control>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SimulateClick(AssociatedObject);
        }

        private void SimulateClick(Control control)
        {
            if (control == null) return;

            var storyboard = (Storyboard)control.FindResource("FlashForegroundStoryboard");

            Storyboard.SetTarget(storyboard, control);
            storyboard.Begin();
        }
    }
}