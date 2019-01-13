using NetMMS.Design;
using NetMMS.Model;
using System;
using System.Windows;

namespace NetMMS.Common {
    public interface IEntityVM : IIdentity<Guid>, IMeasurement, IVisible, IEnable {
        object Entity { get; }
    }

    public abstract class EntityVM<TEntity> : VM, IEntityVM {
        protected Point _pointItem;
        protected Size _sizeItem;
        protected bool _isVisible;
        protected bool _isEnabled;

        protected EntityVM(TEntity entity, SpriteData data) {
            Id = Guid.NewGuid();
            Entity = entity;
            _sizeItem = data.Size;
            _pointItem = data.Point;
        }

        public Guid Id { get; set; }
        public object Entity { get; }

        public double X {
            get { return _pointItem.X; }
            set {
                _pointItem.X = value;
                RaisePropertyChanged(nameof(X));
            }
        }

        public double Y {
            get { return _pointItem.Y; }
            set {
                _pointItem.Y = value;
                RaisePropertyChanged(nameof(Y));
            }
        }

        public double Width {
            get { return _sizeItem.Width; }
            set {
                _sizeItem.Width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }

        public double Height {
            get { return _sizeItem.Height; }
            set {
                _sizeItem.Height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }

        public bool IsVisible {
            get { return _isVisible; }
            set {
                _isVisible = value;
                RaisePropertyChanged(nameof(IsVisible));
            }
        }

        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }
    }
}
