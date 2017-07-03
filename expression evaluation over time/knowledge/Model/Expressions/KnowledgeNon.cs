namespace knowledge.Model.Expressions
{
    public class KnowledgeNon : BaseKnowledge
    {
        public static KnowledgeNon Value { get; }

        static KnowledgeNon()
        {
            Value = new KnowledgeNon();
        }
    }
}