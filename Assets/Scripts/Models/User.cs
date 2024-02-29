using System.Collections.Generic;
using Enums;

namespace Models
{
    public class User
    {
        public readonly Inventory Inventory;
        public Dictionary<(InventoryItems, User), int> PointTable { get; private set; }
        
        public User()
        {
            Inventory = new Inventory();
        }

        public void SetPointTable(Dictionary<(InventoryItems, User), int> pointTable)
        {
            PointTable = pointTable;
        }
    }
}