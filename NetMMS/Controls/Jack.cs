using System.Windows;
using System.Windows.Controls;

namespace NetMMS.CustomControls {
    public class Jack : Control {
        static Jack() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Jack), new FrameworkPropertyMetadata(typeof(Jack)));
        }

        public static readonly DependencyProperty IsConnectionOverProperty =
            DependencyProperty.Register("IsConnectionOver", typeof(bool), typeof(Jack));

        public bool IsConnectionOver {
            get { return (bool)GetValue(IsConnectionOverProperty); }
            set { SetValue(IsConnectionOverProperty, value); }
        }
    }
}