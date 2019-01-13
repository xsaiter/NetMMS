using System.Windows;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NetMMS.Common;

namespace NetMMS.CustomControls {
    public class SelectionAdorner : Adorner {
        Point? _startPoint;
        Point? _endPoint;
        readonly Pen _rubberbandPen;
        readonly ActivityCanvas _canvas;

        public SelectionAdorner(ActivityCanvas canvas, Point? dragStartPoint) : base(canvas) {
            _canvas = canvas;
            _startPoint = dragStartPoint;
            _rubberbandPen = new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 2 }, 1) };
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (!IsMouseCaptured) {
                    CaptureMouse();
                }
                _endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            } else {
                if (IsMouseCaptured) {
                    ReleaseMouseCapture();
                }
            }
            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (IsMouseCaptured) {
                ReleaseMouseCapture();
            }
            var adornerLayer = AdornerLayer.GetAdornerLayer(_canvas);
            if (adornerLayer != null) {
                adornerLayer.Remove(this);
            }
            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc) {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
            if (_startPoint.HasValue && _endPoint.HasValue) {
                dc.DrawRectangle(Brushes.Transparent, _rubberbandPen, new Rect(_startPoint.Value, _endPoint.Value));
            }
        }

        protected virtual void UpdateSelection() {
            if (_startPoint != null && _endPoint != null) {
                var selectionArea = new Rect(_startPoint.Value, _endPoint.Value);
                Registry.SelectionService().UnselectAll();
                var items = Registry.SpriteService().Sprites.Where(q => selectionArea.Contains(new Point(q.X, q.Y)));
                Registry.SelectionService().Select(items, new SelectionDataObserver(true));
            }
        }
    }
}
