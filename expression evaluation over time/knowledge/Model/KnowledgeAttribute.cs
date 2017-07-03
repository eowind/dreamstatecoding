namespace knowledge.Model
{
    public class KnowledgeAttribute : BaseKnowledge
    {
        public string Attribute { get; set; }
        public string Subject { get; set; }
        public KnowledgeAttribute()
            : base(KnowledgeType.Attribute)
        {}
        public override string ToString()
        {
            return $"'{Attribute}'('{Subject}')";
        }
    }
}