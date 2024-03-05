using System;
using Models;

namespace LogicServices.Algorithm
{
    public class SelfBuild
    {
        public int SelfBuildThreshold { get; set; }
        private readonly Random _random;
        public SelfBuild(Random random)
        {
            SelfBuildThreshold = 5;
            _random = random;
        }
        public bool Calculate(Trade trade, User target)
        {
            if (target.Inventory.GetInventoryAmount(trade.RequestedItem) == SelfBuildThreshold) return (_random.NextDouble() > 0.5f);
            else return target.Inventory.GetInventoryAmount(trade.RequestedItem) < SelfBuildThreshold;
        }
    }
}