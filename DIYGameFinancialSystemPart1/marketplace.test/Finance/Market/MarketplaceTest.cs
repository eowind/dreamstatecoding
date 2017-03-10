using marketplace.ui.Finance;
using marketplace.ui.Finance.Market;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace marketplace.test.Finance.Market
{
    [TestClass]
    public class MarketplaceTest
    {

        [TestMethod]
        public void MarketplaceTest_Sell()
        {
            var target = new Marketplace("Iron Market");
            var order = new SellOrder(new Security("Iron Ore", 1ul), 1ul);
            target.Sell(order);
            Assert.AreEqual(1, target.SellOrders.Count);
        }

        [TestMethod]
        public void MarketplaceTest_Sell_Cancel()
        {
            var target = new Marketplace("Iron Market");
            var order = new SellOrder(new Security("Iron Ore", 1ul), 1ul);
            target.Sell(order);
            order.Cancel();
            target.Update();
            Assert.AreEqual(0, target.SellOrders.Count);
        }


        [TestMethod]
        public void MarketplaceTest_Buy()
        {
            var target = new Marketplace("Iron Market");
            var order = new BuyOrder("Iron Ore", 1ul, 100ul, new Account(100ul));
            target.Buy(order);
            Assert.AreEqual(1, target.BuyOrders.Count);
        }

        [TestMethod]
        public void MarketplaceTest_Buy_Cancel()
        {
            var target = new Marketplace("Iron Market");
            var order = new BuyOrder("Iron Ore", 1ul, 100ul, new Account(100ul));
            target.Buy(order);
            order.Cancel();
            target.Update();
            Assert.AreEqual(0, target.BuyOrders.Count);
        }

        [TestMethod]
        public void MarketplaceTest_Update_NoMatch()
        {
            var target = new Marketplace("Iron Market");
            var order = new BuyOrder("Iron Ore", 1ul, 100ul, new Account(100ul));
            target.Buy(order);
            target.Update();
            Assert.AreEqual(1, target.BuyOrders.Count);
        }
        [TestMethod]
        public void MarketplaceTest_Update_MatchExact()
        {
            var target = new Marketplace("Iron Market");
            target.Buy(new BuyOrder("Iron Ore", 1ul, 100ul, new Account(100ul)));
            target.Sell(new SellOrder(new Security("Iron Ore", 100ul), 1ul));
            target.Update();
            Assert.AreEqual(0, target.BuyOrders.Count);
            Assert.AreEqual(0, target.SellOrders.Count);
        }
        [TestMethod]
        public void MarketplaceTest_Update_MatchExact_InvertedOrder()
        {
            var target = new Marketplace("Iron Market");
            target.Sell(new SellOrder(new Security("Iron Ore", 100ul), 1ul));
            target.Buy(new BuyOrder("Iron Ore", 1ul, 100ul, new Account(100ul)));
            target.Update();
            Assert.AreEqual(0, target.BuyOrders.Count);
            Assert.AreEqual(0, target.SellOrders.Count);
        }
    }
}
