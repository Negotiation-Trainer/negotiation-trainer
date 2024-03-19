using Models;

namespace LogicServices.Algorithm
{
    public class Usefulness
    {
        public bool Calculate(Trade trade, Tribe target)
        {
            if (target.Inventory.GetInventoryAmount(trade.OfferedItem) + trade.OfferedAmount > 5) return true;
            return false;
        }
    }
}