using System.Collections.Generic;

namespace knowledge.Model.Expressions
{
    public abstract class AExpression : BaseKnowledge
    {
        public AExpression Parent { get; private set; }
        public AExpression Left { get; }
        public string Operator { get; }
        public AExpression Right { get; }

        public AExpression(BaseKnowledge left, string op, BaseKnowledge right)
        {
            if (left != null)
            {
                var l = left as AExpression;
                Left = l ?? new ExpressionLeaf(left);
            }
            Operator = op;
            if (right != null)
            {
                var r = right as AExpression;
                Right = r ?? new ExpressionLeaf(right);
            }
        }

        public virtual bool IsWellFormed()
        {
            if (Left == null)
                return false;
            if (Right == null)
                return false;
            var result = Left.IsWellFormed();
            if (!Right.IsWellFormed())
                result = false;
            return result;
        }

        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }

        public List<ExpressionLeaf> GetLeafNodes(AExpression parent = null)
        {
            var result = new List<ExpressionLeaf>();
            Parent = parent;
            GetLeafNode(Left, ref result);
            GetLeafNode(Right, ref result);
            return result;
        }

        private void GetLeafNode(AExpression expr,  ref List<ExpressionLeaf> result)
        {
            var exprLeaf = expr as ExpressionLeaf;
            if (exprLeaf == null)
            {
                result.AddRange(expr.GetLeafNodes(this));
            }
            else 
            {
                result.Add(exprLeaf);
            }
        }

        public abstract EvaluationResult TransformEvaluation(Dictionary<ExpressionLeaf, EvaluationResult> facts);
    }
}