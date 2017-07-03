using knowledge.Model;
using knowledge.Model.Contexts;
using knowledge.Model.Expressions;
using knowledge.Model.Objects;
using knowledge.Model.Relative;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace knowledge.test.Expressions
{
    [TestClass]
    public class RelativeAllTest
    {
        [TestMethod]
        public void RelativeAllTest_AddFrame_CurrentUpdated()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr = new ExpressionIs(new KnowledgeRelation { Subject = "Lawyer", Relation = "Occupation of", Target = obj.ToString() });
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void RelativeAllTest_All_True()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr = new ExpressionIs(new KnowledgeRelation
            {
                Subject = "Alice",
                Relation = "Name of",
                Target = obj.ToString()
            });
            var rel = new RelativeAll(expr);
            var actual = target.Evaluate(rel);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void RelativeAllTest_All_False()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr = new ExpressionIs(new KnowledgeRelation
            {
                Subject = "Lawyer",
                Relation = "Occupation of",
                Target = obj.ToString()
            });
            var rel = new RelativeAll(expr);
            var actual = target.Evaluate(rel);
            Assert.AreEqual(EvaluationResult.False, actual);
        }

        [TestMethod]
        public void RelativeAllTest_All_NotSure()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr = new ExpressionIs(new KnowledgeRelation
            {
                Subject = "Undercut",
                Relation = "Hair style of",
                Target = obj.ToString()
            });
            var rel = new RelativeAll(expr);
            var actual = target.Evaluate(rel);
            Assert.AreEqual(EvaluationResult.NotSure, actual);
        }

        private static Person CreateTarget(out Context target)
        {
            var obj = new Person("Alice");
            obj.Occupation = "Student";
            target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            obj.Occupation = "Lawyer";
            target.AddFrame(FrameFactory.Create(obj));
            return obj;
        }
    }
}
