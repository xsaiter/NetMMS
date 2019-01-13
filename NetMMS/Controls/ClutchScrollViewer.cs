using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace NetMMS.CustomControls {
    public class ClutchScrollViewer : ScrollViewer {
        static ClutchScrollViewer() {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ClutchScrollViewer),
                new FrameworkPropertyMetadata(typeof(ClutchScrollViewer)));
        }

        public ClutchScrollViewer() {
            var property = DependencyPropertyDescriptor.FromProperty(ScaleProperty, typeof(ClutchScrollViewer));
            property.AddValueChanged(this, (sender, args) => { });
        }

        public double Scale {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(ClutchScrollViewer), new UIPropertyMetadata(0.0));

        public void ScrollToCenterTarget(Point target) {
            ScrollToHorizontalOffset(target.X - ScrollInfo.ViewportWidth / 2.0);
            ScrollToVerticalOffset(target.Y - ScrollInfo.ViewportHeight / 2.0);
        }
    }
}
