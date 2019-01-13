namespace NetMMS.Common {      
    public class ZoomService : NotifyBase {
        double _zoom = 100;

        public double Zoom {
            get { return _zoom; }
            set {
                _zoom = value;
                RaisePropertyChanged(nameof(Zoom));
            }
        }
    }
}