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
}
