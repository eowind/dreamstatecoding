using System;
using System.Collections.Generic;
using System.Linq;
using knowledge.Model;
using knowledge.Model.Expressions;

namespace knowledge
{
    public class KnowledgeStore : IKnowledgeProvider<string>
    {
        private readonly KnowledgeModel _model;

        public KnowledgeStore()
        {
            _model = new KnowledgeModel();

        }

        public void AddAttribute(KnowledgeAttribute attribute, string source)
        {
            AddAttribute(attribute, new HashSet<string> {source});
        }


        public void AddAttribute(KnowledgeAttribute attribute, HashSet<string> sources)
        {
            if (attribute == null)
                return;
            lock (_model.Attributes)
            {
                KnowledgeAttribute existing;
                _model.Attributes.TryGetValue(attribute.ToString(), out existing);
                if (existing == null)
                {
                    _model.Attributes.Add(attribute.ToString(), attribute);
                }
                else
                {
                    existing.AddSources(sources);
                }
            }
        }

        public void AddRelation(KnowledgeRelation relation, string source)
        {
            AddRelation(relation, new HashSet<string> { source });
        }
        public void AddRelation(KnowledgeRelation relation, HashSet<string> sources)
        {
            if (relation == null)
                return;
            lock (_model.Relations)
            {
                KnowledgeRelation existing;
                _model.Relations.TryGetValue(relation.ToString(), out existing);
                if (existing == null)
                {
                    _model.Relations.Add(relation.ToString(), relation);
                }
                else
                {
                    existing.AddSources(sources);
                }
            }
        }

        public void AddImplication(KnowledgeImplication implication, HashSet<string> sources)
        {
            if (implication == null)
                return;
            lock (_model.Implications)
            {
                KnowledgeImplication existing;
                _model.Implications.TryGetValue(implication.ToString(), out existing);
                if (existing == null)
                {
                    _model.Implications.Add(implication.ToString(), implication);
                }
                else
                {
                    existing.AddSources(sources);
                }
            }
        }

        public HashSet<string> ListAllWith(HashSet<string> attributes)
        {
            HashSet<string> result = null;
            foreach (var attribute in attributes)
            {
                var found = ListAllWith(attribute);
                if (found.Count == 0)
                    break; // nothing matches all attributes

                if (result == null)
                    result = found;
                else
                    result.IntersectWith(found);

                if (result.Count == 0)
                    break; // nothing matches all attributes
            }
            return result ?? new HashSet<string>();
        }
        public HashSet<string> ListAllWith(string attribute)
        {
            return new HashSet<string>(from x in _model.Attributes.Values
                                       where
                                           x.Attribute.Equals(attribute, StringComparison.OrdinalIgnoreCase)
                                       orderby x.Weight
                                       select x.Subject
                );
        }
        public HashSet<string> ListAllAttributes(string variable)
        {
            return new HashSet<string>(from x in _model.Attributes.Values
                                       where
                                           x.Subject.Equals(variable, StringComparison.OrdinalIgnoreCase)
                                       orderby x.Weight
                                       select x.Attribute
                );
        }


        public HashSet<string> ListAllRelated(string relationType, string target)
        {
            var relationTypes = GetAllImplications(relationType);
            return new HashSet<string>(from x in _model.Relations.Values
                                       where
                    relationTypes.Contains(x.Relation)
                    && x.Target.Equals(target, StringComparison.OrdinalIgnoreCase)
                                       orderby x.Weight
                                       select x.Subject
                );
        }

        public HashSet<string> GetAllImplications(string implied)
        {
            var result = new HashSet<string>();
            result.UnionWith(from x in _model.Implications.Values
                             where x.Implied.Equals(implied, StringComparison.OrdinalIgnoreCase)
                             select x.Implicator);
            var chained = new HashSet<string>();
            foreach (var item in result)
            {
                chained.UnionWith(GetAllImplications(item));
            }
            result.UnionWith(chained);
            result.Add(implied);
            return result;
        }

        private List<KnowledgeAttribute> GetAttributes(string variable)
        {
            var attr = (from x in _model.Attributes where x.Value.Subject.Equals(variable) select x.Value).ToList();
            return attr;
        }

        public HashSet<string> GetAllVariables()
        {
            var result = new HashSet<string>();
            lock (_model.Attributes)
            {
                foreach (var attribute in _model.Attributes.Values)
                    result.Add(attribute.Subject);
            }
            lock (_model.Relations)
            {
                foreach (var relation in _model.Relations.Values)
                {
                    result.Add(relation.Subject);
                    result.Add(relation.Target);
                }

            }
            return result;
        }

        public List<string> GetImplicationChain(string variable, string targetAttribute)
        {
            var attr = GetAttributes(variable);
            foreach (var attribute in attr)
            {
                var chain = GetImplicationChainIteration(attribute, targetAttribute);
                if (chain.Any() && chain.Last().Equals(targetAttribute))
                    return chain;
            }
            return new List<string>();
        }
        private List<string> GetImplicationChainIteration(KnowledgeAttribute attribute, string targetAttribute)
        {
            var impl = (from x in
                _model.Implications.Values
                        where
                            x.Implicator.Equals(attribute.Attribute)
                        select x).ToList();
            var chain = new List<string> { attribute.Attribute };
            chain.AddRange(GetImplicationChainRecursive(impl, targetAttribute));
            return chain;
        }


        private List<string> GetImplicationChainRecursive(List<KnowledgeImplication> implications, string targetAttribute)
        {
            var chain = new List<string>();
            foreach (var next in implications)
            {
                if (next.Implied.Equals(targetAttribute))
                {
                    chain.Add(next.Implied);
                    return chain;
                }
                var attr = (from x in _model.Implications where x.Value.Implicator.Equals(next.Implied) select x.Value).ToList();
                var tmp = GetImplicationChainRecursive(attr, targetAttribute);
                if (tmp.Any() && tmp.Last().Equals(targetAttribute))
                {
                    chain.Add(next.Implied);
                    chain.AddRange(tmp);
                    break;
                }
            }
            return chain;
        }

        public bool Exists(BaseKnowledge node)
        {
            if (node is KnowledgeAttribute)
                return ExistsInternal(node as KnowledgeAttribute);
            if (node is KnowledgeRelation)
                return ExistsInternal(node as KnowledgeRelation);
            if (node is KnowledgeImplication)
                return ExistsInternal(node as KnowledgeImplication);
            if (node is KnowledgeNon)
                return true;
            return false;
        }

        private bool ExistsInternal(KnowledgeImplication implication)
        {
            KnowledgeImplication existing;
            return _model.Implications.TryGetValue(implication.ToString(), out existing);
        }

        private bool ExistsInternal(KnowledgeRelation relation)
        {
            KnowledgeRelation existing;
            return _model.Relations.TryGetValue(relation.ToString(), out existing);
        }

        private bool ExistsInternal(KnowledgeAttribute attribute)
        {
            KnowledgeAttribute existing;
            return _model.Attributes.TryGetValue(attribute.ToString(), out existing);
        }

        public EvaluationResult Evaluate(KnowledgeAttribute attr)
        {
            return (from x in _model.Attributes
                where x.Value.Attribute.Equals(attr.Attribute)
                      && x.Value.Subject.Equals(attr.Subject)
                select x).Any()
                ? EvaluationResult.True
                : EvaluationResult.NotSure;
        }
        internal EvaluationResult Evaluate(KnowledgeRelation rel)
        {
            var correctRelationNames =  (from x in _model.Relations
                    where x.Value.Target.Equals(rel.Target)
                          && x.Value.Relation.Equals(rel.Relation)
                    select x).ToList();
            if(!correctRelationNames.Any())
                return EvaluationResult.NotSure;

            return correctRelationNames.Any(x => x.Value.Subject.Equals(rel.Subject))
                ? EvaluationResult.True
                : EvaluationResult.False;
        }
    }
}
