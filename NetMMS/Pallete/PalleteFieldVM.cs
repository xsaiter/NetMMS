using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetMMS.Common;
using NetMMS.Design;
using NetMMS.Model;

namespace NetMMS.Pallete {   
    public class PalleteFieldVM :VM {        
        public PalleteFieldVM() {            
            Items = new ObservableCollection<IPalleteVM> {
                new ComputerPalleteVM(new Computer(), new SpriteData(new Point(), new Size(25,25)))
            };
            PreviewMouseLeftButtonDownCommand = new ActionCommand<MouseButtonEventArgs>(OnPreviewMouseLeftButtonDownCommand);
        }

        public ObservableCollection<IPalleteVM> Items { get; }
        public ActionCommand<MouseButtonEventArgs> PreviewMouseLeftButtonDownCommand { get; }

        protected virtual void OnPreviewMouseLeftButtonDownCommand(MouseButtonEventArgs e) {            
            var presenter = VisualFinder.GetAncestor<ContentPresenter>((DependencyObject)e.OriginalSource);            
            if (presenter != null) {
                if (presenter.DataContext is IPalleteVM item) {
                    DragDrop.DoDragDrop(presenter, new DataObject(SpriteData.FORMAT_DRAG_DROP, item.Entity), DragDropEffects.Copy);
                }
            }
        }        
    }
}
