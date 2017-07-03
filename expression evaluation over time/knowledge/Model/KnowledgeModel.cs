using System.Collections.Generic;

namespace knowledge.Model
{
    public class KnowledgeModel
    {
        public Dictionary<string, KnowledgeAttribute> Attributes { get; set; }
        public Dictionary<string, KnowledgeRelation> Relations { get; set; }
        public Dictionary<string, KnowledgeImplication> Implications { get; set; }

        public KnowledgeModel()
        {
            Attributes = new Dictionary<string, KnowledgeAttribute>();
            Relations = new Dictionary<string, KnowledgeRelation>();
            Implications = new Dictionary<string, KnowledgeImplication>();
        }
    }
}