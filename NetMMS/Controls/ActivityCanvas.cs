using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace NetMMS.CustomControls {
    public class ActivityCanvas : Canvas {
        Point? _startPoint;

        public ActivityCanvas() {
            Instance = this;
        }

        public static ActivityCanvas Instance { get; private set; }

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            _startPoint = e.GetPosition(this);
            e.Handled = true;

            Registry.SelectionService().UnselectAll();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed) {
                _startPoint = null;
            }

            if (_startPoint.HasValue) {
                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                var adorner = new SelectionAdorner(this, _startPoint);
                adornerLayer.Add(adorner);
            }

            e.Handled = true;
        }

        protected override Size MeasureOverride(Size constraint) {
            base.MeasureOverride(constraint);

            var size = new Size();

            foreach (UIElement child in InternalChildren) {
                var transformCollection = ((TransformGroup)child.RenderTransform).Children;

                var translateTransform = transformCollection[0] as TranslateTransform;                

                var left = 0.0;
                var top = 0.0;

                if (translateTransform != null) {
                    left = translateTransform.Value.OffsetX;
                    top = translateTransform.Value.OffsetY;
                }

                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                child.Measure(constraint);

                var desiredSize = child.DesiredSize;

                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height)) {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            size.Width += 10;
            size.Height += 10;

            return size;
        }

        public static readonly DependencyProperty LayerProperty =
            DependencyProperty.Register("Layer", typeof(UIElement), typeof(ActivityCanvas));

        public UIElement Layer {
            get { return (UIElement)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }

        public void RefreshMeasure() {
            InvalidateMeasure();
        }                
    }
}