namespace knowledge.Model.Objects
{
    public class Person : BaseObject
    {
        public string Name { get; set; }
        public string Identification { get; set; }
        public GenderType Gender { get; set; }
        public string Occupation { get; set; }
        public Person(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
        public enum GenderType
        {
            Unknown,
            Male,
            Female,
            Custom,
        }
    }
}