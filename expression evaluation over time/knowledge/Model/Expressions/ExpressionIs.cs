using System;
using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public class ExpressionIs : AExpression
    {
        public ExpressionIs(BaseKnowledge val)
            : base(KnowledgeNon.Value, "", val)
        {
        }

        public override EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts)
        {
            var result = Right.TransformEvaluation(facts);
            switch (result)
            {
                case EvaluationResult.False:
                    return EvaluationResult.False;
                case EvaluationResult.True:
                    return EvaluationResult.True;
                case EvaluationResult.NotSure:
                    return EvaluationResult.NotSure;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}