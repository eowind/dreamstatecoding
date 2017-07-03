using System.Collections.Generic;

namespace knowledge.Model.Objects
{
    public class ObjectCollection : BaseObject
    {
        public string Name { get; set; }
        public List<Person> Persons { get; set; }
        public List<Car> Cars { get; set; }
        public List<Connection> Connections { get; set; }
        public bool IsRealWorld { get; set; }

        public ObjectCollection()
        {
            Persons = new List<Person>();
            Cars = new List<Car>();
            Connections = new List<Connection>();
        }
    }
}
