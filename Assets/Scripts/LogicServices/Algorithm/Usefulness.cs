using System;
using Models;

namespace LogicServices.Algorithm
{
    public class Usefulness
    {
        private readonly Random _random;

        public Usefulness(Random random)
        {
            _random = random;
        }
        public bool Calculate(Trade trade, Tribe target)
        {
            if (target.Inventory.GetInventoryAmount(trade.OfferedItem) + trade.OfferedAmount > 5) return true;
            if (target.Inventory.GetInventoryAmount(trade.OfferedItem) + trade.OfferedAmount == 5) return _random.NextDouble() > 0.5f;
            return false;
        }
    }
}