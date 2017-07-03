using System.Collections.Generic;
using System.Linq;

namespace knowledge.Model
{
    public class BaseKnowledge
    {
        private readonly Dictionary<string, KnowledgeSource> _sources = new Dictionary<string, KnowledgeSource>();
        public double Weight { get; private set; }
        public KnowledgeType Type { get; set; }

        public BaseKnowledge()
        { }

        public BaseKnowledge(KnowledgeType type)
        {
            Type = type;
        }

        public void AddSources(HashSet<string> sources)
        {
            lock (_sources)
            {
                foreach (var sourceName in sources)
                {
                    KnowledgeSource source;
                    if (_sources.TryGetValue(sourceName, out source))
                    {
                        source.Increment();
                    }
                    else
                    {
                        _sources.Add(sourceName, new KnowledgeSource(sourceName));
                    }
                    Weight = _sources.Values.Sum(x => x.Weight);
                }
            }
        }
    }
}