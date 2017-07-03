namespace knowledge.Model.Objects
{
    public class Car : BaseObject
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} [{LicensePlate}]";
        }
    }
}