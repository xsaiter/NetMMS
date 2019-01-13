using System.Collections.ObjectModel;

namespace NetMMS.Design {       
    public class SpriteService {                
        public ObservableCollection<ISpriteVM> Sprites { get; } = new ObservableCollection<ISpriteVM>();
    }
}
