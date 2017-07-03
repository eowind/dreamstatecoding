using System;

namespace knowledge.Model.Objects
{
    public class BaseObject
    {
        public Guid Id { get; }

        public BaseObject()
        {
            Id = Guid.NewGuid();
        }

        protected bool Equals(BaseObject other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseObject) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"o:{GetType().Name}, {Id}";
        }
    }
}