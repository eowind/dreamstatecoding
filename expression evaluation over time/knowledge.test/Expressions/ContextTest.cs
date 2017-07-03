using knowledge.Model;
using knowledge.Model.Contexts;
using knowledge.Model.Expressions;
using knowledge.Model.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace knowledge.test.Expressions
{
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void ContextTest_Evaluate_Not_False()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionNot(new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() });
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Not_True()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionNot(new KnowledgeRelation { Relation = "Name of", Subject = "T", Target = obj.ToString() });
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_And_BothTrue()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "Charlize", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_And_RightFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "T", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_And_LeftFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionAnd(
                new KnowledgeRelation { Relation = "Name of", Subject = "T", Target = obj.ToString() },
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_And_BothFalse()
        {
            var obj = new Person("Charlize") { Gender = Person.GenderType.Female };
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionAnd(
                new KnowledgeRelation { Relation = "Name of", Subject = "T", Target = obj.ToString() },
                new KnowledgeRelation { Relation = "Gender of", Subject = "male", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }
        [TestMethod]
        public void ContextTest_Evaluate_Or_BothTrue()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionOr(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Or_RightFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionOr(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Or_LeftFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionOr(
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() },
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Or_BothFalse()
        {
            var obj = new Person("Charlize") { Gender = Person.GenderType.Female };
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionOr(
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() },
                new KnowledgeRelation { Relation = "Gender of", Subject = "Male", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }


        [TestMethod]
        public void ContextTest_Evaluate_Xor_BothTrue()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionXor(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "Charlize", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Xor_RightFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionXor(
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() },
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Xor_LeftFalse()
        {
            var obj = new Person("Charlize");
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionXor(
                new KnowledgeRelation { Relation = "Name of", Subject = "Kate", Target = obj.ToString() },
                new KnowledgeAttribute { Attribute = "Person", Subject = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.True, actual);
        }

        [TestMethod]
        public void ContextTest_Evaluate_Xor_BothFalse()
        {
            var obj = new Person("Charlize") { Gender = Person.GenderType.Female };
            var target = new Context(obj.ToString());
            target.AddFrame(FrameFactory.Create(obj));
            var expr = new ExpressionXor(
                new KnowledgeRelation { Relation = "Name of", Subject = "T", Target = obj.ToString() },
                new KnowledgeRelation { Relation = "Gender of", Subject = "female", Target = obj.ToString() }
                );
            var actual = target.Evaluate(expr);
            Assert.AreEqual(EvaluationResult.False, actual);
        }
    }
}
