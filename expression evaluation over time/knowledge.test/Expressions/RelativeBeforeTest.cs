using knowledge.Model;
using knowledge.Model.Contexts;
using knowledge.Model.Expressions;
using knowledge.Model.Objects;
using knowledge.Model.Relative;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace knowledge.test.Expressions
{
    [TestClass]
    public class RelativeTest
    {
        [TestMethod]
        public void RelativeBeforeTest_True()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr1 = new ExpressionIs(new KnowledgeRelation { Subject = "Student", Relation = "Occupation of", Target = obj.ToString() });
            var expr2 = new ExpressionIs(new KnowledgeRelation { Subject = "Lawyer", Relation = "Occupation of", Target = obj.ToString() });
            var relative = new RelativeBefore(expr1, expr2);
            var actual = target.Evaluate(relative);
            Assert.AreEqual(EvaluationResult.True, actual);
        }
        [TestMethod]
        public void RelativeBeforeTest_False()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr1 = new ExpressionIs(new KnowledgeRelation { Subject = "Student", Relation = "Occupation of", Target = obj.ToString() });
            var expr2 = new ExpressionIs(new KnowledgeRelation { Subject = "Lawyer", Relation = "Occupation of", Target = obj.ToString() });
            var relative = new RelativeBefore(expr2, expr1);
            var actual = target.Evaluate(relative);
            Assert.AreEqual(EvaluationResult.False, actual);
        }
        [TestMethod]
        public void RelativeBeforeTest_LeftNotSure_NotSure()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr1 = new ExpressionIs(new KnowledgeRelation { Subject = "Undercut", Relation = "Hair style of", Target = obj.ToString() });
            var expr2 = new ExpressionIs(new KnowledgeRelation { Subject = "Lawyer", Relation = "Occupation of", Target = obj.ToString() });
            var relative = new RelativeBefore(expr1, expr2);
            var actual = target.Evaluate(relative);
            Assert.AreEqual(EvaluationResult.NotSure, actual);
        }
        [TestMethod]
        public void RelativeBeforeTest_RightNotSure_NotSure()
        {
            Context target;
            var obj = CreateTarget(out target);

            var expr1 = new ExpressionIs(new KnowledgeRelation { Subject = "Undercut", Relation = "Hair style of", Target = obj.ToString() });
            var expr2 = new ExpressionIs(new KnowledgeRelation { Subject = "Lawyer", Relation = "Occupation of", Target = obj.ToString() });
            var relative = new RelativeBefore(expr2, expr1);
            var actual = target.Evaluate(relative);
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
