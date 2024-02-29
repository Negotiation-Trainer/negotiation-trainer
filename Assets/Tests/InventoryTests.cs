using System.Collections;
using System.Collections.Generic;
using Enums;
using Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Create_Inventory_ReturnsZeroResources()
    {
        Inventory inventory = new Inventory();
        Dictionary<InventoryItems, int> expected = new Dictionary<InventoryItems, int>
        {
            {InventoryItems.Wood,0},
            {InventoryItems.Lenses,0},
            {InventoryItems.Clay,0},
            {InventoryItems.Gold,0},
            {InventoryItems.Steel,0},
            {InventoryItems.Insulation,0},
            {InventoryItems.Fertilizer,0},
            {InventoryItems.Stone,0}
        };
        Assert.AreEqual(expected,inventory.GetInventory());
    }
    
    [Test]
    public void Add_OneResources_ReturnsSameNumber()
    {
        Inventory inventory = new Inventory();
        inventory.AddToInventory(InventoryItems.Wood,1);
        Assert.AreEqual(1,inventory.GetInventoryAmount(InventoryItems.Wood));
    }
    
    [Test]
    public void Add_ZeroResources_ReturnsSameNumber()
    {
        Inventory inventory = new Inventory();
        inventory.AddToInventory(InventoryItems.Wood,0);
        Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
    }
    
    [Test]
    public void Remove_FromEmptyInventory_ReturnsZero()
    {
        Inventory inventory = new Inventory();
        inventory.RemoveFromInventory(InventoryItems.Wood,1);
        Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
    }
    
    [Test]
    public void Remove_FromFilledInventory_ReturnsLowerNumber()
    {
        Inventory inventory = new Inventory();
        inventory.AddToInventory(InventoryItems.Wood,2);
        inventory.RemoveFromInventory(InventoryItems.Wood,2);
        Assert.AreEqual(0,inventory.GetInventoryAmount(InventoryItems.Wood));
    }
    
    [Test]
    public void CanBuild_EmptyInventory_ReturnsFalse()
    {
        Inventory inventory = new Inventory();
        Assert.IsFalse(inventory.CanBuild(InventoryItems.Wood));
    }
    
    [Test]
    public void CanBuild_EnoughInInventory_ReturnsTrue()
    {
        Inventory inventory = new Inventory();
        inventory.AddToInventory(InventoryItems.Wood,10);
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
