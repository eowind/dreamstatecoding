using marketplace.ui.Finance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace marketplace.test.Finance
{
    [TestClass]
    public class AccountTest
    {

        [TestMethod]
        public void AccountTest_CanAfford_Yes()
        {
            var target = new Account(100L);
            var actual = target.CanAfford(1);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AccountTest_CanAfford_No()
        {
            var target = new Account(100L);
            var actual = target.CanAfford(1000);
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AccountTest_CanAfford_Edge_Yes()
        {
            var target = new Account(100L);
            var actual = target.CanAfford(100);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AccountTest_CanAfford_Edge_No()
        {
            var target = new Account(100L);
            var actual = target.CanAfford(1001);
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AccountTest_Deposit()
        {
            var src = new Account(100ul);
            var dest = new Account(0ul);
            var actual = src.DepositInto(dest, 100ul);
            Assert.AreEqual(true, actual);
            Assert.AreEqual(0ul, src.Balance);
            Assert.AreEqual(100ul, dest.Balance);
        }
    }
}
