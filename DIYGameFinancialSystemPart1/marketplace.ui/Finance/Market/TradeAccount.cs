using System;
using System.Collections.Generic;

namespace marketplace.ui.Finance.Market
{
    public class TradeAccount : ITradeAccount
    {
        public event EventHandler<TradeAccount> Updated;
        public IAccount BalanceAccount { get; private set; }

        public ulong ValueOfActiveBuyOrders
        {
            get
            {
                ulong sum = 0;
                foreach (var item in BuyOrders)
                {
                    sum += item.OrderAccount.Balance;
                }
                return sum;
            }
        }
        public Dictionary<string, Security> Securities { get; private set; }
        public List<BuyOrder> BuyOrders { get; private set; }
        public List<SellOrder> SellOrders { get; private set; }

        public TradeAccount(IAccount initialBalance)
            : this(initialBalance, new List<Security>())
        {}

        public TradeAccount(IAccount initialBalance, List<Security> initialSecurities)
        {
            BalanceAccount = initialBalance;
            Securities = new Dictionary<string, Security>();
            foreach (var s in initialSecurities)
            {
                if (Securities.ContainsKey(s.Name))
                    Securities[s.Name].Quantity += s.Quantity;
                else
                    Securities.Add(s.Name, s);
            }
            BuyOrders = new List<BuyOrder>();
            SellOrders = new List<SellOrder>();
        }

        public SellOrder CreateSellOrder(string name, ulong quantity, ulong pricePerItem)
        {
            Security security; 
            if (!Securities.TryGetValue(name, out security))
                return null;
            var split = security.Split(quantity);
            if (split == null)
                return null;
            var order = new SellOrder(split, pricePerItem);
            order.AddCancelAction(CancelSellOrder);
            order.AddExecuteAction(ExecuteSellOrder);
            lock (SellOrders)
            {
                SellOrders.Add(order);
            }
            Updated?.Invoke(this, this);
            return order;
        }

        private void ExecuteSellOrder(SellOrder order)
        {
            order.OrderAccount.DepositInto(BalanceAccount, order.OrderAccount.Balance);
            lock (SellOrders)
            {
                SellOrders.Remove(order);
            }
            Updated?.Invoke(this, this);
        }
        private void CancelSellOrder(SellOrder order)
        {
            Security security;
            if (!Securities.TryGetValue(order.SecurityName, out security))
            {
                Securities.Add(order.SecurityName, order.ForSale);
            }
            else
            {
                Securities[order.SecurityName].Merge(order.ForSale);
            }
            lock (SellOrders)
            {
                SellOrders.Remove(order);
            }
            Updated?.Invoke(this, this);
        }
        public BuyOrder CreateBuyOrder(string name, ulong quantity, ulong pricePerItem)
        {
            var orderAccount = new Account(0);
            if (!BalanceAccount.DepositInto(orderAccount, quantity*pricePerItem))
                return null;
            var order = new BuyOrder(name, pricePerItem, quantity, orderAccount);
            order.AddCancelAction(CancelBuyOrder);
            order.AddExecuteAction(ExecuteBuyOrder);
            lock (BuyOrders)
            {
                BuyOrders.Add(order);
            }
            Updated?.Invoke(this, this);
            return order;
        }

        private void ExecuteBuyOrder(BuyOrder order)
        {
            Security security;
            if (!Securities.TryGetValue(order.SecurityName, out security))
            {
                Securities.Add(order.SecurityName, order.Security);
            }
            else
            {
                Securities[order.SecurityName].Merge(order.Security);
            }
            lock (BuyOrders)
            {
                BuyOrders.Remove(order);
            }
            Updated?.Invoke(this, this);
        }
        private void CancelBuyOrder(BuyOrder order)
        {
            order.OrderAccount.DepositInto(BalanceAccount, order.OrderAccount.Balance);
            lock (BuyOrders)
            {
                BuyOrders.Remove(order);
            }
            Updated?.Invoke(this, this);
        }
    }
}