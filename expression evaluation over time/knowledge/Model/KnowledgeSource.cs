using System;

namespace knowledge.Model
{
    public class KnowledgeSource
    {
        public string Source { get; }
        public double Weight { get; private set; }
        private int _occurances;

        public KnowledgeSource(string source)
        {
            Source = source;
            Increment();
        }

        public void Increment()
        {
            lock (this)
            {
                _occurances++;
                Weight += _occurances * Math.Pow(_occurances, -Math.E);
            }
        }

        protected bool Equals(KnowledgeSource other)
        {
            return string.Equals(Source, other.Source);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KnowledgeSource) obj);
        }

        public override int GetHashCode()
        {
            return Source?.GetHashCode() ?? 0;
        }
    }
}