using System;
using System.Collections.Generic;

namespace marketplace.ui.Finance.Market
{
    public class BuyOrder
    {
        public IAccount OrderAccount { get; private set; }
        public string SecurityName { get; private set; }
        public ulong PricePerItem { get; private set; }
        public ulong Quantity { get; private set; }
        public Security Security { get; set; }

        private readonly List<Action<BuyOrder>> _executeActions;
        private readonly List<Action<BuyOrder>> _cancelActions;
        public BuyOrder(string shoppingFor, ulong pricePerItem, ulong quantity, Account orderAccount)
        {
            Quantity = quantity;
            OrderAccount = orderAccount;
            SecurityName = shoppingFor;
            PricePerItem = pricePerItem;
            _executeActions = new List<Action<BuyOrder>>();
            _cancelActions = new List<Action<BuyOrder>>();
        }
        public void Execute(SellOrder sellOrder)
        {
            OrderAccount.DepositInto(sellOrder.OrderAccount, sellOrder.Quantity * sellOrder.PricePerItem);
            Security = sellOrder.ForSale;
            foreach (var action in _executeActions)
            {
                action(this);
            }
            sellOrder.Execute();
        }
        public void Cancel()
        {
            foreach (var action in _cancelActions)
            {
                action(this);
            }
        }
        public void AddExecuteAction(Action<BuyOrder> action)
        {
            _executeActions.Add(action);
        }

        public void AddCancelAction(Action<BuyOrder> action)
        {
            _cancelActions.Add(action);
        }
        public override string ToString()
        {
            return $"Buy:{SecurityName}: {Quantity} units, {PricePerItem} /unit";
        }
    }
}