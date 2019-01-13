using System.ComponentModel;

namespace NetMMS.Common {
    public class NotifyBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }    

    public class VM : NotifyBase {               
    }
}