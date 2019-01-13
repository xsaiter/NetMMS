using System.Collections.ObjectModel;
using System.Windows;
using NetMMS.Common;
using NetMMS.CustomControls;

namespace NetMMS.Design {    
    public class DesignFieldVM : VM {                       
        public DesignFieldVM() {                        
            DropTargetCommand = new ActionCommand<DragEventArgs>(OnDropTargetCommand);
            DragOverCommand = new ActionCommand<DragEventArgs>(OnDragOverCommand);                        
        }

        public ActionCommand<DragEventArgs> DropTargetCommand { get; }
        public ActionCommand<DragEventArgs> DragOverCommand { get; }        

        protected virtual void OnDropTargetCommand(DragEventArgs e) {
            if (e.Data.GetDataPresent(SpriteData.FORMAT_DRAG_DROP)) {
                var data = e.Data.GetData(SpriteData.FORMAT_DRAG_DROP);
                if (data != null) {
                    var point = e.GetPosition(ActivityCanvas.Instance);
                    var sprite = SpriteFactory.Create(data.GetType(), new SpriteData(new Point(point.X, point.Y)));
                    Sprites.Add(sprite);
                }
            }                      
        }

        protected virtual void OnDragOverCommand(DragEventArgs e) { }
        public ObservableCollection<ISpriteVM> Sprites => Registry.SpriteService().Sprites;
        public ZoomService ZoomService => Registry.ZoomService();
    }
}