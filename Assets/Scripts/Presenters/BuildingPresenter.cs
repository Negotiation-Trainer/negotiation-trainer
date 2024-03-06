using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Models;
using Models.Structures;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingPresenter : MonoBehaviour
{
    [Header("Tribe A")]
    [SerializeField] private GameObject aBrokenHomes;
    [SerializeField] private GameObject aRepairedHomes;
    [SerializeField] private GameObject aObservationPoint;
    [SerializeField] private GameObject aObservatory;
    [SerializeField] private GameObject aTemple;
    [SerializeField] private GameObject aShelter;
    [SerializeField] private GameObject aWarehouse;
    [SerializeField] private GameObject aWaveBreaker;
    [Header("Tribe B")]
    [SerializeField] private GameObject bBrokenHomes;
    [SerializeField] private GameObject bRepairedHomes;
    [SerializeField] private GameObject bObservationPoint;
    [SerializeField] private GameObject bObservatory;
    [SerializeField] private GameObject bTemple;
    [SerializeField] private GameObject bShelter;
    [SerializeField] private GameObject bWarehouse;
    [SerializeField] private GameObject bWaveBreaker;
    [Header("Tribe C")]
    [SerializeField] private GameObject cBrokenHomes;
    [SerializeField] private GameObject cRepairedHomes;
    [SerializeField] private GameObject cObservationPoint;
    [SerializeField] private GameObject cObservatory;
    [SerializeField] private GameObject cTemple;
    [SerializeField] private GameObject cShelter;
    [SerializeField] private GameObject cWarehouse;
    [SerializeField] private GameObject cWaveBreaker;
    
    private GameManager _gameManager;
    private Dictionary<(Tribe, InventoryItems), IBuildable> _buildableStructures;
    /*
     * This class is responsible for handling the construction of buildings.
     *
     * It should be able to:
     * - Place a building on the map at the right position
     * - Keep track of the buildings that are placed
     */
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        Debug.Log("BuildingPresenter started");

        _buildableStructures = new()
        {
            { (_gameManager.Player, InventoryItems.Wood), new Structure(aObservationPoint) },
            { (_gameManager.Player, InventoryItems.Lenses), new Structure(aObservatory) },
            { (_gameManager.Player, InventoryItems.Clay), new Houses(aBrokenHomes, aRepairedHomes) },
            { (_gameManager.Player, InventoryItems.Gold), new Structure(aTemple) },
            { (_gameManager.Player, InventoryItems.Steel), new Structure(aShelter) },
            { (_gameManager.Player, InventoryItems.Insulation), new Structure(aWarehouse) },
            { (_gameManager.Player, InventoryItems.Stone), new Structure(aWaveBreaker) },
            
            { (_gameManager.Cpu1, InventoryItems.Wood), new Structure(bObservationPoint) },
            { (_gameManager.Cpu1, InventoryItems.Clay), new Houses(bBrokenHomes, bRepairedHomes) },
            { (_gameManager.Cpu1, InventoryItems.Gold), new Structure(bTemple) },
            { (_gameManager.Cpu1, InventoryItems.Lenses), new Structure(bObservatory) },
            { (_gameManager.Cpu1, InventoryItems.Steel), new Structure(bShelter) },
            { (_gameManager.Cpu1, InventoryItems.Insulation), new Structure(bWarehouse) },
            { (_gameManager.Cpu1, InventoryItems.Stone), new Structure(bWaveBreaker) },
            
            { (_gameManager.Cpu2, InventoryItems.Wood), new Structure(cObservationPoint) },
            { (_gameManager.Cpu2, InventoryItems.Lenses), new Structure(cObservatory) },
            { (_gameManager.Cpu2, InventoryItems.Clay), new Houses(cBrokenHomes, cRepairedHomes) },
            { (_gameManager.Cpu2, InventoryItems.Gold), new Structure(cTemple) },
            { (_gameManager.Cpu2, InventoryItems.Steel), new Structure(cShelter) },
            { (_gameManager.Cpu2, InventoryItems.Insulation), new Structure(cWarehouse) },
            { (_gameManager.Cpu2, InventoryItems.Stone), new Structure(cWaveBreaker) },
        };
        _gameManager.Player.Inventory.InventoryUpdate += CheckIfBuildingPossible;
        _gameManager.Cpu1.Inventory.InventoryUpdate += CheckIfBuildingPossible;
        _gameManager.Cpu2.Inventory.InventoryUpdate += CheckIfBuildingPossible;
    }
    
    private void CheckIfBuildingPossible(object sender, EventArgs eventArgs)
    {
        // Check if the player has enough resources to build a building

        var items = Enum.GetValues(typeof(InventoryItems));
        foreach (InventoryItems resource in items)
        {
            if (_gameManager.Player.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Player,resource);
                AddBuildingPoints(_gameManager.Player, resource);
            }
            if (_gameManager.Cpu1.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Cpu1,resource);
                AddBuildingPoints(_gameManager.Cpu1, resource);
            }
            if (_gameManager.Cpu2.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Cpu2,resource);
                AddBuildingPoints(_gameManager.Cpu2, resource);
            }
        }
        
    }
    
    private void PlaceBuilding(Tribe tribe, InventoryItems resource)
    {
        _buildableStructures[(tribe,resource)].Build();
    }

    private void AddBuildingPoints(Tribe tribe, InventoryItems resource)
    {
        _gameManager.Player.Points +=  _gameManager.Player.PointTable[(resource, tribe)];
        _gameManager.Cpu1.Points += _gameManager.Cpu1.PointTable[(resource, tribe)];
        _gameManager.Cpu2.Points += _gameManager.Cpu2.PointTable[(resource, tribe)];
    }
}