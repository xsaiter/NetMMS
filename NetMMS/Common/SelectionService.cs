using System;
using System.Collections.Generic;
using System.Linq;

namespace NetMMS.Common {
    public abstract class Mediator<TObserver, T, TData> where TObserver : IObserver<T> {
        protected List<TObserver> _observers = new List<TObserver>();
        public void Attach(TObserver observer) => _observers.Add(observer);
        public void Detach(TObserver observer) => _observers.Remove(observer);
        public void Publish(TObserver sender, TData data) => OnPublish(sender, data);
        protected abstract void OnPublish(TObserver sender, TData e);
    }

    public class SelectionData {
        public SelectionData(bool isShiftCtrl) => IsShiftCtrl = isShiftCtrl;
        public bool IsShiftCtrl { get; }
    }

    public class SelectionDataObserver {
        public SelectionDataObserver(bool select) => Select = select;
        public bool Select { get; }
    }

    public interface ISelectionServiceObserver : IObserver<SelectionDataObserver>, ISelection {
    }

    public class SelectionService : Mediator<ISelectionServiceObserver, SelectionDataObserver, SelectionData> {
        public void Select(IEnumerable<ISelectionServiceObserver> targetItems, SelectionDataObserver dataObserver) {
            targetItems.ToList().ForEach(x => x.OnNext(dataObserver));
            OnItemsGrouped(_observers.Count(x => x.IsSelected) > 1);
        }

        public void SelectAll() {
            _observers.ForEach(x => x.OnNext(new SelectionDataObserver(true)));
            OnItemsGrouped(_observers.Count(x => x.IsSelected) > 1);
        }

        public void UnselectAll() {
            Registry.SpriteService().Sprites.ToList().ForEach(x => x.OnNext(new SelectionDataObserver(false)));
            OnItemsGrouped(false);
        }

        public event EventHandler<bool> ItemsGrouped;

        protected virtual void OnItemsGrouped(bool isGroup) {
            var x = ItemsGrouped;
            if (x != null) {
                x.Invoke(this, isGroup);
            }
        }

        protected override void OnPublish(ISelectionServiceObserver sender, SelectionData e) {
            if (!e.IsShiftCtrl) {
                var sprites = Registry.SpriteService().Sprites;
                sprites.Foreach(x => x.OnNext(new SelectionDataObserver(false)));
            }
            sender.OnNext(new SelectionDataObserver(true));
            OnItemsGrouped(_observers.Count(x => x.IsSelected) > 1);
        }
    }
}
