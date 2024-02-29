using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public User Cpu1 { get; private set; }
    public User Cpu2 { get; private set; }
    public User Player { get; private set; }

    private void Awake()
    {
        Instance = this;

        Cpu1 = new User();
        Cpu2 = new User();
        Player = new User();
        
        SetPointTables();
        FillInventory();
    }

    private void SetPointTables()
    {
        var pointTablePlayer = new Dictionary<(InventoryItems, User), int>
        {
            [(InventoryItems.Wood, Player)] = 10,
            [(InventoryItems.Wood, Cpu1)] = -5,
            [(InventoryItems.Wood, Cpu2)] = 5,

            [(InventoryItems.Lenses, Player)] = 10,
            [(InventoryItems.Lenses, Cpu1)] = 5,
            [(InventoryItems.Lenses, Cpu2)] = 0,

            [(InventoryItems.Clay, Player)] = 10,
            [(InventoryItems.Clay, Cpu1)] = 0,
            [(InventoryItems.Clay, Cpu2)] = -5,

            [(InventoryItems.Gold, Player)] = 10,
            [(InventoryItems.Gold, Cpu1)] = 5,
            [(InventoryItems.Gold, Cpu2)] = 0,

            [(InventoryItems.Steel, Player)] = 10,
            [(InventoryItems.Steel, Cpu1)] = 0,
            [(InventoryItems.Steel, Cpu2)] = 0,

            [(InventoryItems.Insulation, Player)] = 10,
            [(InventoryItems.Insulation, Cpu1)] = 5,
            [(InventoryItems.Insulation, Cpu2)] = 5,

            [(InventoryItems.Fertilizer, Player)] = 10,
            [(InventoryItems.Fertilizer, Cpu1)] = 0,
            [(InventoryItems.Fertilizer, Cpu2)] = 0,

            [(InventoryItems.Stone, Player)] = 10,
            [(InventoryItems.Stone, Cpu1)] = 0,
            [(InventoryItems.Stone, Cpu2)] = 5,
        };

        var pointTableCpu1 = new Dictionary<(InventoryItems, User), int>
        {
            [(InventoryItems.Wood, Player)] = 0,
            [(InventoryItems.Wood, Cpu1)] = 10,
            [(InventoryItems.Wood, Cpu2)] = 5,

            [(InventoryItems.Lenses, Player)] = -5,
            [(InventoryItems.Lenses, Cpu1)] = 10,
            [(InventoryItems.Lenses, Cpu2)] = 0,

            [(InventoryItems.Clay, Player)] = 5,
            [(InventoryItems.Clay, Cpu1)] = 10,
            [(InventoryItems.Clay, Cpu2)] = -5,

            [(InventoryItems.Gold, Player)] = 5,
            [(InventoryItems.Gold, Cpu1)] = 10,
            [(InventoryItems.Gold, Cpu2)] = -5,

            [(InventoryItems.Steel, Player)] = 0,
            [(InventoryItems.Steel, Cpu1)] = 10,
            [(InventoryItems.Steel, Cpu2)] = 5,

            [(InventoryItems.Insulation, Player)] = 5,
            [(InventoryItems.Insulation, Cpu1)] = 10,
            [(InventoryItems.Insulation, Cpu2)] = 5,

            [(InventoryItems.Fertilizer, Player)] = 0,
            [(InventoryItems.Fertilizer, Cpu1)] = 10,
            [(InventoryItems.Fertilizer, Cpu2)] = 0,

            [(InventoryItems.Stone, Player)] = 0,
            [(InventoryItems.Stone, Cpu1)] = 10,
            [(InventoryItems.Stone, Cpu2)] = 0,
        };
        
        var pointTableCpu2 = new Dictionary<(InventoryItems, User), int>
        {
            [(InventoryItems.Wood, Player)] = 0,
            [(InventoryItems.Wood, Cpu1)] = -5,
            [(InventoryItems.Wood, Cpu2)] = 10,
            
            [(InventoryItems.Lenses, Player)] = -5,
            [(InventoryItems.Lenses, Cpu1)] = 5,
            [(InventoryItems.Lenses, Cpu2)] = 10,
            
            [(InventoryItems.Clay, Player)] = 5,
            [(InventoryItems.Clay, Cpu1)] = 0,
            [(InventoryItems.Clay, Cpu2)] = 10,
            
            [(InventoryItems.Gold, Player)] = 0,
            [(InventoryItems.Gold, Cpu1)] = 0,
            [(InventoryItems.Gold, Cpu2)] = 10,
            
            [(InventoryItems.Steel, Player)] = -5,
            [(InventoryItems.Steel, Cpu1)] = 0,
            [(InventoryItems.Steel, Cpu2)] = 10,
            
            [(InventoryItems.Insulation, Player)] = 0,
            [(InventoryItems.Insulation, Cpu1)] = -5,
            [(InventoryItems.Insulation, Cpu2)] = 10,
            
            [(InventoryItems.Fertilizer, Player)] = 0,
            [(InventoryItems.Fertilizer, Cpu1)] = 0,
            [(InventoryItems.Fertilizer, Cpu2)] = 10,
            
            [(InventoryItems.Stone, Player)] = 0,
            [(InventoryItems.Stone, Cpu1)] = 5,
            [(InventoryItems.Stone, Cpu2)] = 10,
        };
        
        Player.SetPointTable(pointTablePlayer);
        Cpu1.SetPointTable(pointTableCpu1);
        Cpu2.SetPointTable(pointTableCpu2);
    }
    
    private void FillInventory()
    {
        Player.Inventory.AddToInventory(InventoryItems.Wood, 6);
        Player.Inventory.AddToInventory(InventoryItems.Lenses, 6);
        Player.Inventory.AddToInventory(InventoryItems.Clay, 1);
        Player.Inventory.AddToInventory(InventoryItems.Gold, 6);
        Player.Inventory.AddToInventory(InventoryItems.Steel, 2);
        Player.Inventory.AddToInventory(InventoryItems.Insulation, 0);
        Player.Inventory.AddToInventory(InventoryItems.Fertilizer, 2);
        Player.Inventory.AddToInventory(InventoryItems.Stone, 2);
        
        Cpu1.Inventory.AddToInventory(InventoryItems.Wood, 4);
        Cpu1.Inventory.AddToInventory(InventoryItems.Lenses, 2);
        Cpu1.Inventory.AddToInventory(InventoryItems.Clay, 4);
        Cpu1.Inventory.AddToInventory(InventoryItems.Gold, 5);
        Cpu1.Inventory.AddToInventory(InventoryItems.Steel, 5);
        Cpu1.Inventory.AddToInventory(InventoryItems.Insulation, 3);
        Cpu1.Inventory.AddToInventory(InventoryItems.Fertilizer, 3);
        Cpu1.Inventory.AddToInventory(InventoryItems.Stone, 6);
        
        Cpu2.Inventory.AddToInventory(InventoryItems.Wood, 1);
        Cpu2.Inventory.AddToInventory(InventoryItems.Lenses, 2);
        Cpu2.Inventory.AddToInventory(InventoryItems.Clay, 6);
        Cpu2.Inventory.AddToInventory(InventoryItems.Gold, 1);
        Cpu2.Inventory.AddToInventory(InventoryItems.Steel, 4);
        Cpu2.Inventory.AddToInventory(InventoryItems.Insulation, 7);
        Cpu2.Inventory.AddToInventory(InventoryItems.Fertilizer, 4);
        Cpu2.Inventory.AddToInventory(InventoryItems.Stone, 5);
    }
}
