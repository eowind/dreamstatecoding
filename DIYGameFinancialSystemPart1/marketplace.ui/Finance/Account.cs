using System;

namespace marketplace.ui.Finance
{
    public class Account : IAccount
    {
        public Guid Id { get; }
        public ulong Balance { get; private set; }

        public Account(ulong initialAmount)
        {
            Id = Guid.NewGuid();
            Balance = initialAmount;
        }

        public bool CanAfford(ulong x)
        {
            return Balance >= x;
        }

        public bool DepositInto(IAccount dest, ulong amount)
        {
            lock (this)
            {
                if (!CanAfford(amount))
                    return false;
                SubtractBalance(amount);
            }
            lock (dest)
            {
                dest.AddBalance(amount);
            }
            return true;
        }

        public void AddBalance(ulong amount)
        {
            lock (this)
            {
                Balance += amount;
            }
        }
        private void SubtractBalance(ulong amount)
        {
            Balance -= amount;
        }
    }
}