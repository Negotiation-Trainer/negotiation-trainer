using System;
using Models;

namespace LogicServices.Algorithm
{
    public class SelfBuild
    {
        private readonly int _selfBuildThreshold;
        private readonly Random _random;
        public SelfBuild(int selfBuildThreshold, Random random)
        {
            _selfBuildThreshold = selfBuildThreshold;
            _random = random;
        }
        public bool Calculate(Trade trade, User target)
        {
            if (target.Inventory.GetInventoryAmount(trade.RequestedItem) == _selfBuildThreshold) return (_random.NextDouble() > 0.5f);
            else return target.Inventory.GetInventoryAmount(trade.RequestedItem) < _selfBuildThreshold;
        }
    }
}