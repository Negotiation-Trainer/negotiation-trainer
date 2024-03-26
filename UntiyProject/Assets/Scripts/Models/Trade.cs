using Enums;

namespace Models
{
    public class Trade
    {
        public InventoryItems RequestedItem { get; private set; }
        public int RequestedAmount { get; private set; }
        
        public InventoryItems OfferedItem { get; private set; }
        public int OfferedAmount { get; private set; }

        public Trade(InventoryItems requestedItem, int requestedAmount, InventoryItems offeredItem, int offeredAmount)
        {
            this.RequestedItem = requestedItem;
            this.RequestedAmount = requestedAmount;
            this.OfferedItem = offeredItem;
            this.OfferedAmount = offeredAmount;
        }
    }
}