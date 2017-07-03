using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionOr : AExpression
    {
        public ExpressionOr(BaseKnowledge left, BaseKnowledge right)
            : base(left, "Or", right)
        {}

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            var leftResult = Left.TransformEvaluation(facts);
            if (leftResult == EvaluationResult.True)
                return EvaluationResult.True;

            var rightResult = Right.TransformEvaluation(facts);
            if (rightResult == EvaluationResult.True)
                return EvaluationResult.True;

            return EvaluationResult.False;
        }
    }
}