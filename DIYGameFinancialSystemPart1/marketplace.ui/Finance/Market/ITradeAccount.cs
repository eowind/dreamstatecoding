using System;
using System.Collections.Generic;

namespace marketplace.ui.Finance.Market
{
    public interface ITradeAccount
    {
        event EventHandler<TradeAccount> Updated;
        IAccount BalanceAccount { get; }
        ulong ValueOfActiveBuyOrders { get; }
        Dictionary<string, Security> Securities { get; }
        List<BuyOrder> BuyOrders { get; }
        List<SellOrder> SellOrders { get; }

        SellOrder CreateSellOrder(string name, ulong quantity, ulong pricePerItem);
        BuyOrder CreateBuyOrder(string name, ulong quantity, ulong pricePerItem);
    }
    
}