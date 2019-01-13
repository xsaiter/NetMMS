namespace NetMMS.Model {
    public interface IIdentity<T> {
        T Id { get; set; }
    }

    public class Computer : IIdentity<int> {
        public int Id { get; set; }
        public string NetBiosName { get; set; }
        public string Domain { get; set; }    
    }

    public class Connector : IIdentity<int> {
        public int Id { get; set; }
    }

    public class Printer : IIdentity<int> {
        public int Id { get; set; }
        public string Name { get; set; }        
    }
}
