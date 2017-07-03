using System;
using System.Collections.Generic;

namespace knowledge.Model.Objects
{
    public class ObjectFile
    {
        public Dictionary<Guid, ObjectCollection> ObjectCollections { get; set; }
        public ObjectCollection CurrentObjectCollection { get; set; }
        public List<Person> Persons => CurrentObjectCollection.Persons;
        public List<Car> Cars => CurrentObjectCollection.Cars;
        public List<Connection> Connections => CurrentObjectCollection.Connections;
        public ObjectFile()
        {
            ObjectCollections = new Dictionary<Guid, ObjectCollection>();
        }
        
        public void SetCurrent(ObjectCollection objectCollection)
        {
            CurrentObjectCollection = objectCollection;
        }

    }
}