using NetMMS.Common;
using NetMMS.Design;
using NetMMS.Model;

namespace NetMMS.Pallete {
    public interface IPalleteVM : IEntityVM { }

    public class PalleteVM<TEntity> : EntityVM<TEntity>, IPalleteVM {
        public PalleteVM(TEntity entity, SpriteData data) : base(entity, data) { }
    }

    public class ComputerPalleteVM : PalleteVM<Computer> {
        public ComputerPalleteVM(Computer entity, SpriteData data) : base(entity, data) { }
    }
}
