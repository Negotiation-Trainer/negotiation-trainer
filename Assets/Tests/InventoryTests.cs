using System.Collections.Generic;
using Enums;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class InventoryTests
    {
        [Test]
        public void Add_OneResources_ReturnsSameNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,1);
            
            //Then
            Assert.AreEqual(1,inventory.GetInventoryAmount(InventoryItems.Wood));
        }
    
        [Test]
        public void Add_ZeroResources_ReturnsSameNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,0);
            
            //Then
            Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
        }
    
        [Test]
        public void Remove_FromEmptyInventory_ReturnsZero()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.RemoveFromInventory(InventoryItems.Wood,1);
            
            //Then
            Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
        }
    
        [Test]
        public void Remove_FromFilledInventory_ReturnsLowerNumber()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,2);
            inventory.RemoveFromInventory(InventoryItems.Wood,2);
            
            //Then
            Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
        }
    
        [Test]
        public void CanBuild_EmptyInventory_ReturnsFalse()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //Then
            Assert.IsFalse(inventory.CanBuild(InventoryItems.Wood));
        }
    
        [Test]
        public void CanBuild_EnoughInInventory_ReturnsTrue()
        {
            //Given
            Inventory inventory = new Inventory();
            
            //When
            inventory.AddToInventory(InventoryItems.Wood,10);
            
            //Then
            Assert.IsTrue(inventory.CanBuild(InventoryItems.Wood));
        }
    
        [Test]
        public void Event_InventoryUpdate_Add_Invoked()
        {
            //Given
            Inventory inventory = new Inventory();
            bool eventInvoked = false;
            inventory.InventoryUpdate += (sender, args) => eventInvoked = true;
        
            //When
            inventory.AddToInventory(InventoryItems.Wood,1);
        
            //Then
            Assert.IsTrue(eventInvoked);
        }
    
        [Test]
        public void Event_InventoryUpdate_Remove_Invoked()
        {
            //Given
            Inventory inventory = new Inventory();
            inventory.AddToInventory(InventoryItems.Wood,1);
            bool eventInvoked = false;
            inventory.InventoryUpdate += (sender, args) => eventInvoked = true;
        
            //When
            inventory.RemoveFromInventory(InventoryItems.Wood,1);
        
            //Then
            Assert.IsTrue(eventInvoked);
        }
    
        [Test]
        public void Inventory_ToString_ReturnsCorrectString()
        {
            //Given
            Inventory inventory = new Inventory();
            inventory.AddToInventory(InventoryItems.Wood,1);
            inventory.AddToInventory(InventoryItems.Stone,2);
        
            //When
            string expected = "Wood: 1 Lenses: 0 Clay: 0 Gold: 0 Steel: 0 Insulation: 0 Fertilizer: 0 Stone: 2";
        
            //Then
            Assert.AreEqual(expected,inventory.ToString());
        }
    }
}
