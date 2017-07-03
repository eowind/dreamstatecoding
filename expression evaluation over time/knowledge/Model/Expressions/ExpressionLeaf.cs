using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionLeaf : AExpression
    {
        public BaseKnowledge Leaf { get; set; }
        public ExpressionLeaf(BaseKnowledge leaf) : base(null, "leaf", null)
        {
            Leaf = leaf;
        }

        public override bool IsWellFormed()
        {
            return Leaf != null;
        }

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            return facts[this];
        }
    }
}
