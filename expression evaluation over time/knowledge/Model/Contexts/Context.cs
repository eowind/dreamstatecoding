using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using knowledge.Model.Expressions;
using knowledge.Model.Relative;

namespace knowledge.Model.Contexts
{
    public class Context
    {
        public Guid Id { get; }
        public string Name { get; }

        private readonly List<KnowledgeStore> _frames;
        public IReadOnlyCollection<KnowledgeStore> Frames => new ReadOnlyCollection<KnowledgeStore>(_frames);
        public KnowledgeStore CurrentFrame => _frames.Last();
        public Context(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            _frames = new List<KnowledgeStore> {};
        }

        public void AddFrame(KnowledgeStore frame)
        {
            _frames.Add(frame);
        }

        public EvaluationResult Evaluate(AExpression expression, int frameStart = -1)
        {
            if (frameStart == -1)
                frameStart = _frames.Count - 1;
            var facts = new Dictionary<ExpressionLeaf, EvaluationResult>();
            var leafNodes = expression.GetLeafNodes();
            foreach (var node in leafNodes)
            {
                var leaf = node.Leaf;
                var attr = leaf as KnowledgeAttribute;
                var rel = leaf as KnowledgeRelation;
                if (!(attr != null | rel != null))
                    continue;
                for (int i = frameStart; i >= 0; i--)
                {
                    var frame = _frames[i];
                    var result = attr != null ? frame.Evaluate(attr) : frame.Evaluate(rel);

                    if (result == EvaluationResult.NotSure)
                        continue;
                    facts.Add(node, result);
                    break;
                }
            }
            if (!facts.Any())
                return EvaluationResult.NotSure;
            if (facts.Values.Any(x => x == EvaluationResult.NotSure))
                return EvaluationResult.NotSure;
            return expression.TransformEvaluation(facts);
        }


        public EvaluationResult Evaluate(IRelative expression)
        {
            return expression.Evaluate(this);
        }


    }
}
