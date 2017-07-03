namespace knowledge.Model
{
    public class KnowledgeRelation : BaseKnowledge
    {
        public string Relation { get; set; }
        public string Subject { get; set; }
        public string Target { get; set; }
        public KnowledgeRelation()
            : base(KnowledgeType.Relation)
        { }
        public override string ToString()
        {
            return $"'['{Subject}' {Relation} '{Target}']";
        }
    }
}