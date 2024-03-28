using System;
using System.Collections.Generic;
using Enums;

namespace Models
{
    public class Inventory
    {
        public event EventHandler InventoryUpdate;
        private readonly Dictionary<InventoryItems, int> _inventory = new();


        public Inventory()
        {
            foreach (InventoryItems item in Enum.GetValues(typeof(InventoryItems)))
            {
                _inventory.Add(item, 0);
            }
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
            if(_inventory[item] == 0) return;
            _inventory[item] -= amount;
            InventoryUpdate?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateInventory()
        {
            InventoryUpdate?.Invoke(this,EventArgs.Empty);
        }

        public new string ToString()
        {
            return $"Wood: {_inventory[InventoryItems.Wood]} Lenses: {_inventory[InventoryItems.Lenses]} Clay: {_inventory[InventoryItems.Clay]} Gold: {_inventory[InventoryItems.Gold]} Steel: {_inventory[InventoryItems.Steel]} Insulation: {_inventory[InventoryItems.Insulation]} Fertilizer: {_inventory[InventoryItems.Fertilizer]} Stone: {_inventory[InventoryItems.Stone]}";
        }
    }
}