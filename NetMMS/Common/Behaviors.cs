using NetMMS.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NetMMS.Common {
    public interface IMeasurement {
        double Width { get; set; }
        double Height { get; set; }
    }    

    public interface IVisible {
        bool IsVisible { get; set; }
    }

    public interface IEnable {
        bool IsEnabled { get; set; }
    }             

    public interface ISelection {
        bool IsSelected { get; set; }
    }      

    public interface ILinks<TLink, in TId> where TLink : IIdentity<TId> {
        void AddLink(TLink link);
        void RemoveLink(TLink link);
        ObservableCollection<TLink> GetLinks { get; }
    }

    public interface IParent<T> {
        T Parent { get; set; }
    }

    public interface IComposite<T> {
        void AddComponent(T component);
        void RemoveComponent(T component);
        IEnumerable<T> Children { get; }
    }
}
