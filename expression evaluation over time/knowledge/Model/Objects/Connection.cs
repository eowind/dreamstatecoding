namespace knowledge.Model.Objects
{
    public class Connection : BaseObject
    {
        public BaseObject A { get; set; }
        public BaseObject B { get; set; }
        public ConnectionDirection Direction { get; set; }
        public Connection(BaseObject a, BaseObject b)
        {
            A = a;
            B = b;
            Direction = ConnectionDirection.A2B;
        }

        public override string ToString()
        {
            string dir = string.Empty;
            switch (Direction)
            {
                case ConnectionDirection.A2B:
                    dir = "  => ";
                    break;
                case ConnectionDirection.B2A:
                    dir = " <=  ";
                    break;
                case ConnectionDirection.Both:
                    dir = " <=> ";
                    break;
            }
            return $"{A}{dir}{B}";
        }

        public enum ConnectionDirection
        {
            A2B,
            B2A,
            Both
        }
    }
}