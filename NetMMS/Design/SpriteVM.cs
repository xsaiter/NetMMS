using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using NetMMS.Common;
using NetMMS.CustomControls;
using NetMMS.Model;

namespace NetMMS.Design {
    public interface ISpriteVM : IEntityVM, IParent<ISpriteVM>, ILinks<ISpriteVM, Guid>, ISelectionServiceObserver {
        double X { get; set; }
        double Y { get; set; }
        bool IsDragConnectionOver { get; set; }
        bool IsConnectionSource { get; set; }
    }

    public class SpriteData {
        public SpriteData(Point point, Size size) {
            Point = point;
            Size = size;
        }
        public SpriteData(Point point) : this(point, new Size(50, 50)) { }
        public Point Point { get; }
        public Size Size { get; }
        public const string FORMAT_DRAG_DROP = "formatDragDropEntity";
    }

    public class SpriteVM<TEntity> : EntityVM<TEntity>, ISpriteVM {
        bool _isDragConnectionOver;
        bool _isConnectionSource;
        protected readonly ObservableCollection<ISpriteVM> _links = new ObservableCollection<ISpriteVM>();
        protected bool _isSelected;
        Point? _startPointConnector;        

        public SpriteVM(TEntity entity, SpriteData data) : base(entity, data) {
            MoveSpriteCommand = new ActionCommand<DragDeltaEventArgs>(OnMoveSprite);
            PreviewMouseDownCommand = new ActionCommand<MouseButtonEventArgs>(OnPreviewMouseDown);

            TopResizeSpriteCommand = new ActionCommand<DragDeltaEventArgs>(OnTopResizeSprite);
            BottomResizeItemCommand = new ActionCommand<DragDeltaEventArgs>(OnBottomResizeSprite);

            MoveJackCommand = new ActionCommand<MouseEventArgs>(OnMoveJack);
            DownJackCommand = new ActionCommand<MouseEventArgs>(OnDownJack);

            Registry.SelectionService().Attach(this);
        }

        public ActionCommand<DragDeltaEventArgs> MoveSpriteCommand { get; }
        public ActionCommand<MouseButtonEventArgs> PreviewMouseDownCommand { get; }

        public ActionCommand<DragDeltaEventArgs> TopResizeSpriteCommand { get; }
        public ActionCommand<DragDeltaEventArgs> BottomResizeItemCommand { get; }

        public ActionCommand<MouseEventArgs> MoveJackCommand { get; }
        public ActionCommand<MouseEventArgs> DownJackCommand { get; }

        protected virtual void OnMoveSprite(DragDeltaEventArgs e) {
            Move(e);
            RefreshMeasure();
        }

        void Move(DragDeltaEventArgs e) {
            if (X + e.HorizontalChange < 0 && Y + e.VerticalChange < 0) {
                return;
            }
            if (X + e.HorizontalChange > 0) {
                X += e.HorizontalChange;
            }
            if (Y + e.VerticalChange > 0) {
                Y += e.VerticalChange;
            }
        }

        void OnPreviewMouseDown(MouseButtonEventArgs e) {
            var data = new SelectionData((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None);
            Registry.SelectionService().Publish(this, data);
        }

        const double MIN_HEIGHT = 20;
        const double MIN_WIDTH = 20;

        bool CanNotVerticalResize(double change) => Height - change < MIN_HEIGHT;

        void OnTopResizeSprite(DragDeltaEventArgs e) {
            if(e.VerticalChange > 0.0 && CanNotVerticalResize(e.VerticalChange)) {                
                return;                
            } 
            Height -= e.VerticalChange;
            Y += e.VerticalChange;
            RefreshMeasure();
        }

        void OnBottomResizeSprite(DragDeltaEventArgs e) {
            if(e.VerticalChange < 0.0 && CanNotVerticalResize(-e.VerticalChange)) {
                return;
            }
            Height += e.VerticalChange;
            RefreshMeasure();
        }

        protected virtual void RefreshMeasure() {
            ActivityCanvas.Instance.RefreshMeasure();
        }

        protected virtual void OnMoveJack(MouseEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) {
                _startPointConnector = null;
                return;
            }

            var jack = VisualFinder.GetAncestor<Jack>((DependencyObject)e.OriginalSource);

            if (_startPointConnector.HasValue && jack != null) {
                var adornerLayer = AdornerLayer.GetAdornerLayer(ActivityCanvas.Instance);
                if (adornerLayer != null) {
                    var adorner = new ConnectorAdorner(ActivityCanvas.Instance, _startPointConnector, jack, this);
                    adornerLayer.Add(adorner);
                    e.Handled = true;
                }
            }
        }

        protected virtual void OnDownJack(MouseEventArgs e) {
            IsConnectionSource = true;
            _startPointConnector = e.GetPosition(ActivityCanvas.Instance);
            e.Handled = true;
        }

        public void OnNext(SelectionDataObserver value) {
            IsSelected = value.Select;
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }


        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public bool IsSelected {
            get { return _isSelected; }
            set {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public ISpriteVM Parent { get; set; }

        public void AddLink(ISpriteVM link) {
            _links.Add(link);
        }

        public void RemoveLink(ISpriteVM link) {
            _links.Remove(link);
        }

        public ObservableCollection<ISpriteVM> GetLinks {
            get { return _links; }
        }

        public bool IsDragConnectionOver {
            get { return _isDragConnectionOver; }
            set {
                _isDragConnectionOver = value;
                RaisePropertyChanged(nameof(IsDragConnectionOver));
            }
        }

        public bool IsConnectionSource {
            get { return _isConnectionSource; }
            set {
                _isConnectionSource = value;
                RaisePropertyChanged(nameof(IsConnectionSource));
            }
        }
    }

    public class ComputerVM : SpriteVM<Computer> {
        public ComputerVM(Computer entity, SpriteData data)
            : base(entity, data) {
        }
    }

    public class PrinterVM : SpriteVM<Printer> {
        public PrinterVM(Printer entity, SpriteData data)
            : base(entity, data) {
        }
    }
}
