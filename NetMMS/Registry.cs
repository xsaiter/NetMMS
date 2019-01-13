using NetMMS.Common;
using NetMMS.Design;
using NetMMS.Model;

namespace NetMMS {
    public class Registry {      
        static readonly EntityAccess _entityAccess = new EntityAccess();
        static readonly SpriteService _spriteService = new SpriteService();
        static readonly ZoomService _zoomService = new ZoomService();
        static readonly SelectionService _selectionService = new SelectionService();

        public static EntityAccess EntityAccess() => _entityAccess;
        public static SpriteService SpriteService() => _spriteService;        
        public static ZoomService ZoomService() => _zoomService;
        public static SelectionService SelectionService() => _selectionService;
    }
}
