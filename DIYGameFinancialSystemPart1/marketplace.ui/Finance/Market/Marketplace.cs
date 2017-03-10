using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace marketplace.ui.Finance.Market
{
    public class Marketplace
    {
        public event EventHandler<Marketplace> Updated; 
        public string Name { get; set; }
        private readonly Dictionary<string, List<SellOrder>> _sellOrders;
        private readonly Dictionary<string, List<BuyOrder>> _buyOrders;
        private readonly List<SellOrder> _sellOrdersToBeRemoved;
        private readonly List<BuyOrder> _buyOrdersToBeRemoved;

        public IReadOnlyCollection<SellOrder> SellOrders => new ReadOnlyCollection<SellOrder>((from list in _sellOrders.Values
            from x in list
            select x).ToList());
        public IReadOnlyCollection<BuyOrder> BuyOrders => new ReadOnlyCollection<BuyOrder>((from list in _buyOrders.Values
                                                                                        from x in list
                                                                                        select x).ToList());

        public Marketplace(string name)
        {
            Name = name;
            _sellOrders = new Dictionary<string, List<SellOrder>>();
            _buyOrders = new Dictionary<string, List<BuyOrder>>();
            _sellOrdersToBeRemoved = new List<SellOrder>();
            _buyOrdersToBeRemoved = new List<BuyOrder>();
        }


        public void Update()
        {
            bool updated = false;
            lock(_sellOrders)
            {
                lock (_buyOrders)
                {
                    foreach (var buyName in _buyOrders)
                    {
                        List<SellOrder> sellOrders;
                        if (!_sellOrders.TryGetValue(buyName.Key, out sellOrders))
                            continue;
                        // naive
                        foreach (var buyOrder in buyName.Value)
                        {
                            foreach (var sellOrder in sellOrders)
                            {
                                if (buyOrder.Quantity == sellOrder.Quantity
                                    && buyOrder.PricePerItem == sellOrder.PricePerItem)
                                {
                                    updated = true;
                                    buyOrder.Execute(sellOrder);
                                }
                            }
                        }
                    }
                    foreach (var order in _sellOrdersToBeRemoved)
                    {
                        _sellOrders[order.SecurityName].Remove(order);
                    }
                    foreach (var order in _buyOrdersToBeRemoved)
                    {
                        _buyOrders[order.SecurityName].Remove(order);
                    }

                }
            }
            if (updated)
            {
                Updated?.Invoke(this, this);
            }
        }

        public void Sell(SellOrder order)
        {
            lock (_sellOrders)
            {
                if(_sellOrders.ContainsKey(order.ForSale.Name))
                    _sellOrders[order.ForSale.Name].Add(order);
                else
                    _sellOrders.Add(order.ForSale.Name, new List<SellOrder> { order });
                order.AddCancelAction(RemoveSell);
                order.AddExecuteAction(RemoveSell);
            }
        }

        private void RemoveSell(SellOrder order)
        {
            _sellOrdersToBeRemoved.Add(order);
        }

        public void Buy(BuyOrder order)
        {
            lock (_buyOrders)
            {
                if (_buyOrders.ContainsKey(order.SecurityName))
                    _buyOrders[order.SecurityName].Add(order);
                else
                    _buyOrders.Add(order.SecurityName, new List<BuyOrder> { order });
                order.AddCancelAction(RemoveBuy);
                order.AddExecuteAction(RemoveBuy);
            }
        }

        private void RemoveBuy(BuyOrder order)
        {
            _buyOrdersToBeRemoved.Add(order);
        }
    }
}