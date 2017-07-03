namespace knowledge.Model
{
    public class KnowledgeImplication : BaseKnowledge
    {
        public string Implicator { get; set; }
        public string Implied { get; set; }
        public KnowledgeImplication()
            : base(KnowledgeType.Implication)
        { }
        public override string ToString()
        {
            return $"'{Implicator}' => '{Implied}'";
        }
    }
}