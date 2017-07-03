using knowledge.Model.Contexts;
using knowledge.Model.Expressions;

namespace knowledge.Model.Relative
{
    public interface IRelative
    {
        EvaluationResult Evaluate(Context context);
    }
}