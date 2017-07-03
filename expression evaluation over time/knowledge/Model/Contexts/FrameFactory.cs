using System.Collections.Generic;
using knowledge.Model.Objects;

namespace knowledge.Model.Contexts
{
    public static class FrameFactory
    {

        public static KnowledgeStore Create(BaseObject root)
        {
            var frame = new KnowledgeStore();
            frame.AddAttribute(new KnowledgeAttribute
            {
                Attribute = root.GetType().Name,
                Subject = root.ToString()
            }, root.ToString());
            var fields = GetAllProperties(root);
            foreach (var field in fields)
            {
                frame.AddRelation(new KnowledgeRelation
                {
                    Subject = field.Value,
                    Relation = $"{field.Key} of",
                    Target = root.ToString()
                }, root.ToString());
            }
            return frame;
        }


        private static Dictionary<string, string> GetAllProperties(object obj)
        {
            var d = new Dictionary<string, string>();
            var properties = obj.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var val = prop.GetValue(obj);
                if (val == null)
                    val = string.Empty;
                d.Add(prop.Name, val.ToString());
            }
            return d;
        }
    }
}