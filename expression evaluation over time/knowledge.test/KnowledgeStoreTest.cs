using System.Collections.Generic;
using System.Linq;
using knowledge.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace knowledge.test
{
    [TestClass]
    public class KnowledgeStoreTest
    {
        [TestMethod]
        public void KnowledgeTest_AttributeCatNone()
        {
            var target = new KnowledgeStore();
            var actual = target.ListAllWith("cat");
            Assert.AreEqual(0, actual.Count, "cats");
        }

        [TestMethod]
        public void KnowledgeTest_AttributeCatOne()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> {"test"});
            var actual = target.ListAllWith("cat");
            Assert.AreEqual(1, actual.Count, "cats");
            Assert.AreEqual("Pixel", actual.Single());
        }

        [TestMethod]
        public void KnowledgeTest_AttributeCatTwo()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            var actual = target.ListAllWith("cat");
            Assert.AreEqual(2, actual.Count, "cats");
        }

        [TestMethod]
        public void KnowledgeTest_AttributeDualAttribute()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "female", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "male", Subject = "Tiger" }, new HashSet<string> { "test" });
            var actual = target.ListAllWith(new HashSet<string> { "cat", "female"});
            Assert.AreEqual(1, actual.Count, "cats");
            Assert.AreEqual("Pixel", actual.Single());
        }
        
        [TestMethod]
        public void KnowledgeTest_RelationBrother()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            target.AddRelation(new KnowledgeRelation { Relation = "brotherof", Subject = "Tiger", Target = "Pixel"}, new HashSet<string> { "test" });
            var actual = target.ListAllRelated("brotherof", "Pixel");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Tiger", actual.Single());
        }
        
        [TestMethod]
        public void KnowledgeTest_RelationImplicationBrotherSibling()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            target.AddRelation(new KnowledgeRelation { Relation = "brotherof", Subject = "Tiger", Target = "Pixel" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication {Implicator = "brotherof", Implied = "siblingof" }, new HashSet<string> { "test" });
            var actual = target.ListAllRelated("siblingof", "Pixel");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Tiger", actual.Single());
        }

        [TestMethod]
        public void KnowledgeTest_RelationImplicationChainingFamily()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            target.AddRelation(new KnowledgeRelation { Relation = "brotherof", Subject = "Tiger", Target = "Pixel" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "brotherof", Implied = "siblingof" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "siblingof", Implied = "familyof" }, new HashSet<string> { "test" });
            var actual = target.ListAllRelated("familyof", "Pixel");
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Tiger", actual.Single());
        }

        [TestMethod]
        public void KnowledgeTest_GetAllVariablesNone()
        {
            var target = new KnowledgeStore();
            var actual = target.GetAllVariables();
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void KnowledgeTest_GetAllVariablesFromAttributes()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });

            var actual = target.GetAllVariables();
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains("Pixel"));
            Assert.IsTrue(actual.Contains("Tiger"));
        }

        [TestMethod]
        public void KnowledgeTest_ListAllAttributes()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "comfy", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Tiger" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "fluffy", Subject = "Tiger" }, new HashSet<string> { "test" });

            var actual = target.ListAllAttributes("Pixel");
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains("cat"));
            Assert.IsTrue(actual.Contains("comfy"));
        }

        [TestMethod]
        public void KnowledgeTest_GetAllVariablesFromRelations()
        {
            var target = new KnowledgeStore();
            target.AddRelation(new KnowledgeRelation { Relation = "brotherOf", Subject = "Tiger", Target = "Pixel" }, new HashSet<string> { "test" });

            var actual = target.GetAllVariables();
            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains("Tiger"));
            Assert.IsTrue(actual.Contains("Pixel"));
        }


        [TestMethod]
        public void KnowledgeTest_GetImplicationChainNoMatch()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });

            var actual = target.GetImplicationChain("Pixel", "object");
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void KnowledgeTest_GetImplicationChainDirectHit()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "animal", Implied = "object" }, new HashSet<string> { "test" });

            var actual = target.GetImplicationChain("Pixel", "cat");

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("cat", actual.First());
        }

        [TestMethod]
        public void KnowledgeTest_GetImplicationChain()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "cat", Implied = "animal" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "animal", Implied = "object" }, new HashSet<string> { "test" });

            var actual = target.GetImplicationChain("Pixel", "object");

            var expected = new List<string> { "cat", "animal", "object" };
            Assert.AreEqual(3, actual.Count);
            for (int i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void KnowledgeTest_GetImplicationChainWithGarbageBranches()
        {
            var target = new KnowledgeStore();
            target.AddAttribute(new KnowledgeAttribute { Attribute = "cat", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddAttribute(new KnowledgeAttribute { Attribute = "spotted", Subject = "Pixel" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "cat", Implied = "animal" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "cat", Implied = "furry" }, new HashSet<string> { "test" });
            target.AddImplication(new KnowledgeImplication { Implicator = "animal", Implied = "object" }, new HashSet<string> { "test" });

            var actual = target.GetImplicationChain("Pixel", "object");

            var expected = new List<string> { "cat", "animal", "object" };
            Assert.AreEqual(3, actual.Count);
            for (int i = 0; i < actual.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }


    }
}
