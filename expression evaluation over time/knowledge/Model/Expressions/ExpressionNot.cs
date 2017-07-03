using System;
using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionNot : AExpression
    {
        public ExpressionNot(BaseKnowledge val)
            : base(KnowledgeNon.Value, "!", val)
        {
        }

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            var result = Right.TransformEvaluation(facts);
            switch (result)
            {
                case EvaluationResult.False:
                    return EvaluationResult.True;
                case EvaluationResult.True:
                    return EvaluationResult.False;
                case EvaluationResult.NotSure:
                    return EvaluationResult.NotSure;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}