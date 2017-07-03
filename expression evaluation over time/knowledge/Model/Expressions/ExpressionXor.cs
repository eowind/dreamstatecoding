using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionXor : AExpression
    {
        public ExpressionXor(BaseKnowledge left, BaseKnowledge right)
            : base(left, "Xor", right)
        {}

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            var leftResult = Left.TransformEvaluation(facts);
            var rightResult = Right.TransformEvaluation(facts);

            if ((leftResult == EvaluationResult.True && rightResult == EvaluationResult.False)
                || (leftResult == EvaluationResult.False && rightResult == EvaluationResult.True))
                return EvaluationResult.True;
            
            return EvaluationResult.False;
        }
    }
}