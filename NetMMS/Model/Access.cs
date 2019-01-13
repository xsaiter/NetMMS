using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetMMS.Model {  
    public interface IRepository<T, TId> where T : IIdentity<TId> {
        T GetById(TId id);
        T GetByCondition(Func<T, bool> condition);
        IEnumerable<T> All();
        IEnumerable<T> AllByCondition(Func<T, bool> condition);        
        void Add(T entity);
        void Remove(T entity);
    }

    public class Repository<T, TId> : IRepository<T, TId> where T : IIdentity<TId> {
        protected readonly ObservableCollection<T> _entities = new ObservableCollection<T>();                

        public virtual void Add(T entity) {
            _entities.Add(entity);
        }

        public virtual void Remove(T entity) {
            _entities.Remove(entity);
        }        

        public virtual IEnumerable<T> All() {
            return _entities;
        }

        public IEnumerable<T> AllByCondition(Func<T, bool> condition) {
            return All().Where(condition);
        }        

        public T GetByCondition(Func<T, bool> condition) {
            return All().SingleOrDefault(condition);
        }

        public T GetById(TId id) {
            return _entities.SingleOrDefault(x => x.Id.Equals(id));
        }
    }

    public interface IConnectorRepository : IRepository<Connector, int> {
    }

    public class ConnectorRepository : Repository<Connector, int>, IConnectorRepository {                            
    }

    public interface IComputerRepository : IRepository<Computer, int> {
    }

    public class ComputerRepository : Repository<Computer, int>, IComputerRepository {                            
    }

    public interface IPrinterRepository : IRepository<Printer, int> {
    }

    public class PrinterRepository : Repository<Printer, int>, IPrinterRepository {        
    }

    public class EntityAccess {
        IComputerRepository _computers;
        IPrinterRepository _printers;

        public IComputerRepository Computers {
            get {
                if (_computers == null) {
                    _computers = new ComputerRepository();
                }
                return _computers;
            }
        }

        public IPrinterRepository Printers {
            get {
                if (_printers == null) {
                    _printers = new PrinterRepository();
                }
                return _printers;
            }
        }
    }
}
