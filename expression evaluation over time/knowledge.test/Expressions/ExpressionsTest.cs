using knowledge.Model;
using knowledge.Model.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace knowledge.test.Expressions
{
    [TestClass]
    public class ExpressionsTest
    {
        [TestMethod]
        public void ExpressionTest_IsWellFormed_Single_True()
        {
            var target = new ExpressionAnd(
                new KnowledgeAttribute {Attribute = "cat", Subject = "pixel"},
                new KnowledgeAttribute {Attribute = "cat", Subject = "tiger"}
                );

            Assert.IsTrue(target.IsWellFormed());
        }

        [TestMethod]
        public void ExpressionTest_IsWellFormed_Single_LeftNull_False()
        {
            var target = new ExpressionAnd(
                null,
                new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" }
                );

            Assert.IsFalse(target.IsWellFormed());
        }

        [TestMethod]
        public void ExpressionTest_IsWellFormed_Single_RightNull_False()
        {
            var target = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                null
                );

            Assert.IsFalse(target.IsWellFormed());
        }

        [TestMethod]
        public void ExpressionTest_IsWellFormed_LeftNested_True()
        {
            var target = new ExpressionAnd(
                new ExpressionAnd(
                    new KnowledgeAttribute {Attribute = "cat", Subject = "pixel"},
                    new KnowledgeAttribute {Attribute = "cat", Subject = "prime"}
                    ),
                new KnowledgeAttribute {Attribute = "cat", Subject = "tiger"}
                );

            Assert.IsTrue(target.IsWellFormed());
        }

        [TestMethod]
        public void ExpressionTest_IsWellFormed_RightNested_True()
        {
            var target = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" },
                new ExpressionAnd(
                    new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                    new KnowledgeAttribute { Attribute = "cat", Subject = "prime" }
                    )
                );

            Assert.IsTrue(target.IsWellFormed());
        }


        [TestMethod]
        public void ExpressionTest_GetLeafNodes_SingleLevel()
        {
            var target = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" }
                );
            var actual = target.GetLeafNodes();
            Assert.AreEqual(2, actual.Count);
        }

        [TestMethod]
        public void ExpressionTest_GetLeafNodes_TwoLevel_Left()
        {
            var target = new ExpressionAnd(
                new ExpressionAnd(
                    new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                    new KnowledgeAttribute { Attribute = "cat", Subject = "prime" }
                    ),
                new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" }
                );
            var actual = target.GetLeafNodes();
            Assert.AreEqual(3, actual.Count);
        }
        [TestMethod]
        public void ExpressionTest_GetLeafNodes_TwoLevel_Right()
        {
            var target = new ExpressionAnd(
                new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" },
                new ExpressionAnd(
                    new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                    new KnowledgeAttribute { Attribute = "cat", Subject = "prime" }
                    )
                );
            var actual = target.GetLeafNodes();
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void ExpressionTest_GetLeafNodes_TwoLevel_Both()
        {
            var target = new ExpressionAnd(
                new ExpressionAnd(
                    new KnowledgeAttribute { Attribute = "cat", Subject = "tiger" },
                    new KnowledgeAttribute { Attribute = "cat", Subject = "prime" }
                    ),
                new ExpressionAnd(
                    new KnowledgeAttribute { Attribute = "cat", Subject = "pixel" },
                    new KnowledgeAttribute { Attribute = "sneaky", Subject = "pixel" }
                    )
                );
            var actual = target.GetLeafNodes();
            Assert.AreEqual(4, actual.Count);
        }
    }
}
