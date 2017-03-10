using System.Collections.Generic;
using marketplace.ui.Finance;
using marketplace.ui.Finance.Market;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace marketplace.test.Finance.Market
{
    [TestClass]
    public class TradeAccountTest
    {
        [TestMethod]
        public void TradeAccountTest_CreateBuyOrder_EdgeAfford_OK()
        {
            var target = new TradeAccount(new Account(100ul));
            var actual = target.CreateBuyOrder("Iron Ore", 100ul, 1ul);
            Assert.IsNotNull(actual);
            Assert.AreEqual("Iron Ore", actual.SecurityName);
            Assert.AreEqual(100ul, actual.Quantity);
            Assert.AreEqual(1ul, actual.PricePerItem);
            Assert.AreEqual(0ul, target.BalanceAccount.Balance);
            Assert.AreEqual(1, target.BuyOrders.Count);
        }

        [TestMethod]
        public void TradeAccountTest_CreateBuyOrder_EdgeAfford_Fail()
        {
            var target = new TradeAccount(new Account(100ul));
            var actual = target.CreateBuyOrder("Iron Ore", 101ul, 1ul);
            Assert.IsNull(actual);
            Assert.AreEqual(100ul, target.BalanceAccount.Balance);
            Assert.AreEqual(0, target.BuyOrders.Count);
        }

        [TestMethod]
        public void TradeAccountTest_CancelBuyOrder()
        {
            var target = new TradeAccount(new Account(100ul));
            var actual = target.CreateBuyOrder("Iron Ore", 100ul, 1ul);
            actual.Cancel();
            Assert.AreEqual(100ul, target.BalanceAccount.Balance);
            Assert.AreEqual(0, target.BuyOrders.Count);
        }

        [TestMethod]
        public void TradeAccountTest_ExecuteBuyOrder()
        {
            var target = new TradeAccount(new Account(100ul));
            var actual = target.CreateBuyOrder("Iron Ore", 100ul, 1ul);
            actual.Execute(new SellOrder(new Security("Iron Ore", 100ul), 1ul));
            Assert.AreEqual(0ul, target.BalanceAccount.Balance);
            Assert.AreEqual(0, target.BuyOrders.Count);
        }

        [TestMethod]
        public void TradeAccountTest_CreateSellOrder()
        {
            var target = new TradeAccount(new Account(100ul), new List<Security> { new Security("Iron Ore", 100ul) });
            var actual = target.CreateSellOrder("Iron Ore", 100ul, 1ul);
            Assert.IsNotNull(actual);
            Assert.AreEqual("Iron Ore", actual.SecurityName);
            Assert.AreEqual(100ul, actual.Quantity);
            Assert.AreEqual(1ul, actual.PricePerItem);
            Assert.AreEqual(100ul, target.BalanceAccount.Balance);
            Assert.AreEqual(1, target.SellOrders.Count);
        }

        [TestMethod]
        public void TradeAccountTest_CreateSellOrder_Fail_EdgeQuantity()
        {
            var target = new TradeAccount(new Account(100ul), new List<Security> { new Security("Iron Ore", 100ul) });
            var actual = target.CreateSellOrder("Iron Ore", 101ul, 1ul);
            Assert.IsNull(actual);
        }
        [TestMethod]
        public void TradeAccountTest_CancelSellOrder()
        {
            var target = new TradeAccount(new Account(100ul), new List<Security> { new Security("Iron Ore", 100ul) });
            var actual = target.CreateSellOrder("Iron Ore", 100ul, 1ul);
            actual.Cancel();
            Assert.AreEqual(0, target.SellOrders.Count);
        }
        [TestMethod]
        public void TradeAccountTest_ExecuteSellOrder_BalanceUpdated()
        {
            var target = new TradeAccount(new Account(100ul), new List<Security> { new Security("Iron Ore", 100ul) });
            var actual = target.CreateSellOrder("Iron Ore", 100ul, 1ul);

            var buyAccount = new TradeAccount(new Account(100ul));
            var buyOrder = buyAccount.CreateBuyOrder("Iron Ore", 100ul, 1ul);
            buyOrder.Execute(actual);

            Assert.AreEqual(200ul, target.BalanceAccount.Balance);
        }
    }
}
