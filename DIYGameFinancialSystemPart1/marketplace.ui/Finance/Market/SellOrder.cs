using System;
using System.Collections.Generic;

namespace marketplace.ui.Finance.Market
{
    public class SellOrder
    {
        public IAccount OrderAccount { get; private set; }
        public string SecurityName { get { return ForSale.Name; } }
        public Security ForSale { get; private set; }
        public ulong PricePerItem { get; set; }
        public ulong Quantity { get { return ForSale.Quantity; } }
        private readonly List<Action<SellOrder>> _executeActions;
        private readonly List<Action<SellOrder>> _cancelActions;
        public SellOrder(Security forSale, ulong pricePerItem)
        {
            ForSale = forSale;
            PricePerItem = pricePerItem;
            OrderAccount = new Account(0);
            _executeActions = new List<Action<SellOrder>>();
            _cancelActions = new List<Action<SellOrder>>();
        }
        public void Execute()
        {
            foreach (var action in _executeActions)
            {
                action(this);
            }
        }
        public void Cancel()
        {
            foreach (var action in _cancelActions)
            {
                action(this);
            }
        }

        public void AddExecuteAction(Action<SellOrder> action)
        {
            _executeActions.Add(action);
        }

        public void AddCancelAction(Action<SellOrder> action)
        {
            _cancelActions.Add(action);
        }
        public override string ToString()
        {
            return $"Sell:{ForSale.Name}: {ForSale.Quantity} units, {PricePerItem} /unit";
        }
    }
}