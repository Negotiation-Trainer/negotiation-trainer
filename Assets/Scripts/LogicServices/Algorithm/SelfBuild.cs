using Models;
using UnityEngine;

namespace LogicServices.Algorithm
{
    public class SelfBuild
    {
        private readonly byte _selfBuildThreshold;
        public SelfBuild(byte selfBuildThreshold)
        {
            _selfBuildThreshold = selfBuildThreshold;
        }
        public bool Calculate(Trade trade, User target)
        {
            if (target.Inventory.GetInventoryAmount(trade.RequestedItem) == _selfBuildThreshold) return (Random.value > 0.5f);
            else return target.Inventory.GetInventoryAmount(trade.RequestedItem) < _selfBuildThreshold;
        }
    }
}