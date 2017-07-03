using knowledge.Model.Contexts;
using knowledge.Model.Expressions;

namespace knowledge.Model.Relative
{
    public class RelativeBefore : IRelative
    {
        private readonly AExpression _left;
        private readonly AExpression _right;

        public RelativeBefore(AExpression left, AExpression right)
        {
            _left = left;
            _right = right;
        }

        public EvaluationResult Evaluate(Context context)
        {
            int rightFrameIndex;
            var rightResult = EvaluateExpression(context, _right, out rightFrameIndex);
            int leftFrameIndex;
            var leftResult = EvaluateExpression(context, _left, out leftFrameIndex);

            if (leftResult == EvaluationResult.NotSure || rightResult == EvaluationResult.NotSure)
                return EvaluationResult.NotSure;

            if (leftResult == EvaluationResult.True && rightResult == EvaluationResult.True)
            {
                return leftFrameIndex < rightFrameIndex ? EvaluationResult.True : EvaluationResult.False;
            }
            return EvaluationResult.False;
        }

        private EvaluationResult EvaluateExpression(Context context, AExpression expression, out int frameIndex)
        {
            var result = EvaluationResult.NotSure;
            for (frameIndex = context.Frames.Count - 1; frameIndex >= 0; frameIndex--)
            {
                result = context.Evaluate(expression, frameIndex);
                if (result == EvaluationResult.True)
                {
                    return EvaluationResult.True;
                }
            }
            return result;
        }
    }
}