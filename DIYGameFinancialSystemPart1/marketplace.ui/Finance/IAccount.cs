using System;

namespace marketplace.ui.Finance
{
    public interface IAccount
    {
        Guid Id { get; }
        ulong Balance { get; }
        bool CanAfford(ulong x);
        bool DepositInto(IAccount dest, ulong amount);
        void AddBalance(ulong amount);
    }
}