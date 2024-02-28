using System;
using System.Collections.Generic;
using Enums;

namespace Models
{
    public class Inventory
    {
        public event EventHandler InventoryUpdate;
        private Dictionary<InventoryItems, int> _inventory = new();


        public Inventory()
        {
            foreach (InventoryItems item in Enum.GetValues(typeof(InventoryItems)))
            {
                _inventory.Add(item, 0);
            }
        }
        
        public Dictionary<InventoryItems, int> GetInventory()
        {
            return _inventory;
        }

        public int GetInventoryAmount(InventoryItems item)
        {
            return _inventory[item];
        }

        public void AddToInventory(InventoryItems item, int amount)
        {
            _inventory[item] += amount;
            InventoryUpdate?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveFromInventory(InventoryItems item, int amount)
        {
            _inventory[item] -= amount;
            InventoryUpdate?.Invoke(this, EventArgs.Empty);
        }

        public bool CanBuild(InventoryItems item)
        {
            return _inventory[item] >= 10;
        }

        public new string ToString()
        {
            return $"Wood: {_inventory[InventoryItems.Wood]} Stone: {_inventory[InventoryItems.Stone]} Gold: {_inventory[InventoryItems.Gold]}";
        }
    }
}