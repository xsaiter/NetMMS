using NetMMS.Common;
using NetMMS.Design;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace NetMMS.CustomControls {
    public class ConnectorAdorner : Adorner {
        readonly UIElement _adornedElement;
        readonly Point? _startPoint;
        Point? _endPoint;
        readonly Pen _rubberbandPen = new Pen(Brushes.Red, 0.5) { DashStyle = new DashStyle(new double[] { 2 }, 1) };
        ISpriteVM _spriteTarget;
        readonly ISpriteVM _spriteSource;
        readonly Jack _jackSource;
        Jack _jackTarget;
        readonly List<DependencyObject> _hitResultsList = new List<DependencyObject>();

        public ConnectorAdorner(UIElement adornedElement, Point? startPoint, Jack jackSource, ISpriteVM spriteSource)
            : base(adornedElement) {
            _adornedElement = adornedElement;
            _startPoint = startPoint;
            _jackSource = jackSource;
            _spriteSource = spriteSource;

            Cursor = Cursors.Cross;

            _jackSource.IsConnectionOver = true;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (!IsMouseCaptured) {
                    CaptureMouse();
                }
                _endPoint = e.GetPosition(this);
                HitTest(_endPoint.Value);
                InvalidateVisual();
            } else {
                if (IsMouseCaptured) {
                    ReleaseMouseCapture();
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) {
            if (IsMouseCaptured) {
                ReleaseMouseCapture();
            }
            Cursor = Cursors.None;
            var adornerLayer = AdornerLayer.GetAdornerLayer(_adornedElement);
            if (adornerLayer != null) {
                adornerLayer.Remove(this);
            }
            if (_spriteTarget != null) {
                _spriteTarget.IsDragConnectionOver = false;
            }
            if (_jackSource != null) {
                _jackSource.IsConnectionOver = false;
            }
            _spriteSource.IsConnectionSource = false;
        }

        protected override void OnRender(DrawingContext dc) {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
            if (_startPoint.HasValue && _endPoint.HasValue) {
                dc.DrawLine(_rubberbandPen, _startPoint.Value, _endPoint.Value);
            }
        }

        void HitTest(Point hitPoint) {
            if (_spriteTarget != null) {
                _spriteTarget.IsDragConnectionOver = false;
                _spriteTarget = null;
            }

            if (_jackTarget != null) {
                _jackTarget.IsConnectionOver = false;
                _jackTarget = null;
            }

            VisualTreeHelper.HitTest(_adornedElement,
                new HitTestFilterCallback(HitTestFilter),
                result => HitTestResultBehavior.Continue,
                new PointHitTestParameters(hitPoint));

            if (_adornedElement.InputHitTest(hitPoint) is DependencyObject inputElement) {
                var contentControl = VisualFinder.GetAncestor<ContentControl>(inputElement);
                if (contentControl != null) {
                    _spriteTarget = contentControl.DataContext as ISpriteVM;
                    if (_spriteTarget != null) {
                        _spriteTarget.IsDragConnectionOver = true;
                    }
                }
            }
        }

        HitTestFilterBehavior HitTestFilter(DependencyObject o) {
            if (o.GetType() == typeof(Jack)) {
                _jackTarget = (Jack)o;
                if (ReferenceEquals(_jackTarget, _jackSource)) {
                    return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
                }
                _jackTarget.IsConnectionOver = true;

                var contentControl = VisualFinder.GetAncestor<ContentControl>(_jackTarget);
                _spriteTarget = contentControl.DataContext as ISpriteVM;
                if (_spriteTarget != null) {
                    _spriteTarget.IsDragConnectionOver = true;
                }

                return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
            }

            return HitTestFilterBehavior.Continue;
        }

        bool IsHitTestOnJack(Point hitPoint) {
            _hitResultsList.Clear();

            var filterCallback = new HitTestFilterCallback(x => {
                return x.GetType() == typeof(Jack) ?
                HitTestFilterBehavior.ContinueSkipSelfAndChildren : HitTestFilterBehavior.Continue;
            });

            var resultCallback = new HitTestResultCallback(x => {
                _hitResultsList.Add(x.VisualHit);
                return HitTestResultBehavior.Continue;
            });

            VisualTreeHelper.HitTest(_adornedElement, filterCallback, resultCallback, new PointHitTestParameters(hitPoint));

            return _hitResultsList.Count > 0;
        }
    }
}
