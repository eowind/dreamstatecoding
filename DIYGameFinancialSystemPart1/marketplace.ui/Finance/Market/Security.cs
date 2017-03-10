namespace marketplace.ui.Finance.Market
{
    public class Security
    {
        public string Name { get; set; }
        public ulong Quantity { get; set; }

        public Security(string name, ulong quantity)
        {
            Name = name;
            Quantity = quantity;
        }
        private Security()
        { }
        public Security Split(ulong quantity)
        {
            lock (this)
            {
                if (quantity > Quantity)
                    return null;
                Quantity -= quantity;
                return new Security
                {
                    Name = Name,
                    Quantity = quantity
                };
            }
        }
        public bool Merge(Security other)
        {
            if (!Name.Equals(other.Name))
                return false;
            ulong quantity;
            lock (other)
            {
                quantity = other.Quantity;
                other.Quantity = 0;
            }
            lock (this)
            {
                Quantity += quantity;
            }
            return true;
        }

        public override string ToString()
        {
            return $"{Name}: {Quantity}";
        }
    }
}