using marketplace.ui.Finance.Market;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace marketplace.test.Finance.Market
{
    [TestClass]
    public class SecurityTest
    {

        [TestMethod]
        public void SecurityTest_Split_OK()
        {
            var target = new Security("Iron Ore", 1000ul);
            var actual = target.Split(1);
            Assert.IsNotNull(actual);
            Assert.AreEqual(target.Name, actual.Name);
            Assert.AreEqual(1ul, actual.Quantity);
            Assert.AreEqual(999ul, target.Quantity);
        }

        [TestMethod]
        public void SecurityTest_Split_EdgeOK()
        {
            var target = new Security("Iron Ore", 1000ul);
            var actual = target.Split(1000);
            Assert.IsNotNull(actual);
            Assert.AreEqual(target.Name, actual.Name);
            Assert.AreEqual(1000ul, actual.Quantity);
            Assert.AreEqual(0ul, target.Quantity);
        }
        [TestMethod]
        public void SecurityTest_Split_EdgeFail()
        {
            var target = new Security("Iron Ore", 1000ul);
            var actual = target.Split(1001ul);
            Assert.IsNull(actual);
            Assert.AreEqual(1000ul, target.Quantity);
        }

        [TestMethod]
        public void SecurityTest_Merge_OK()
        {
            var target = new Security("Iron Ore", 1000);
            var other = new Security("Iron Ore", 1000);
            var ok =target.Merge(other);

            Assert.AreEqual(true, ok);
            Assert.AreEqual(2000ul, target.Quantity);
            Assert.AreEqual(0ul, other.Quantity);
        }
        [TestMethod]
        public void SecurityTest_Merge_Fail()
        {
            var target = new Security("Iron Ore", 1000);
            var other = new Security("Silver Ore", 1000);
            var ok = target.Merge(other);

            Assert.AreEqual(false, ok);
            Assert.AreEqual(1000ul, target.Quantity);
            Assert.AreEqual(1000ul, other.Quantity);
        }
    }
}
