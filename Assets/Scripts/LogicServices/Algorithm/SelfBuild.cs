using Models;
using UnityEngine;

namespace LogicServices.Algorithm
{
    public class SelfBuild
    {
        private int _border;
        public SelfBuild(int border)
        {
            _border = border;
        }
        public bool Calculate(Trade trade, User target)
        {
            if (target.Inventory.GetInventoryAmount(trade.RequestedItem) == _border) return (Random.value > 0.5f);
            else return target.Inventory.GetInventoryAmount(trade.RequestedItem) < _border;
        }
    }
}