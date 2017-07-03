using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionAnd : AExpression
    {
        public ExpressionAnd(BaseKnowledge left, BaseKnowledge right)
            : base(left, "And", right)
        { }

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            var leftResult = Left.TransformEvaluation(facts);
            if (leftResult != EvaluationResult.True)
                return EvaluationResult.False;

            var rightResult = Right.TransformEvaluation(facts);
            if(rightResult != EvaluationResult.True)
                return EvaluationResult.False;

            return EvaluationResult.True;
        }
    }
}
