using System.Collections.Generic;
using Enums;

namespace Models
{
    public class Tribe
    {
        public readonly Inventory Inventory;
        public string Name { get; private set; }
        public int Points { get; set; }
        public Dictionary<(InventoryItems, Tribe), int> PointTable { get; set; }
        
        public Tribe(string name)
        {
            Name = name;
            Points = 0;
            Inventory = new Inventory();
        }
    }
}