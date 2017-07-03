using knowledge.Model.Contexts;
using knowledge.Model.Expressions;

namespace knowledge.Model.Relative
{
    public class RelativeAll : IRelative
    {
        private readonly AExpression _expression;

        public RelativeAll(AExpression expression)
        {
            _expression = expression;
        }

        public EvaluationResult Evaluate(Context context)
        {
            var result = EvaluationResult.NotSure;
            for (int frameIndex = context.Frames.Count - 1; frameIndex >= 0; frameIndex--)
            {
                result = context.Evaluate(_expression, frameIndex);
                if(result == EvaluationResult.False)
                    return EvaluationResult.False;
            }
            return result;
        }
    }
}