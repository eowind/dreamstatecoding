using System.Collections.Generic;

namespace knowledge
{
    public interface IKnowledgeProvider<T>
    {
        HashSet<T> ListAllWith(HashSet<string> attributes);
        HashSet<T> ListAllAttributes(string variable);
        HashSet<T> ListAllWith(string attribute);
        HashSet<T> ListAllRelated(string relationType, string variable);
        HashSet<T> GetAllImplications(string implied);
        HashSet<T> GetAllVariables();
        List<T> GetImplicationChain(string variable, string targetAttribute);
    }
}