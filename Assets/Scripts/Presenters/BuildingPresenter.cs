using System;
using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;

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
    private Dictionary<(Tribe, InventoryItems), List<GameObject>> _buildableStructures;
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
            {(_gameManager.Player, InventoryItems.Wood), new List<GameObject>() {aObservationPoint}},
            {(_gameManager.Player, InventoryItems.Lenses), new List<GameObject>() {aObservatory}},
            {(_gameManager.Player, InventoryItems.Clay), new List<GameObject>() {aBrokenHomes, aRepairedHomes}},
            {(_gameManager.Player, InventoryItems.Gold), new List<GameObject>() {aTemple}},
            {(_gameManager.Player, InventoryItems.Steel), new List<GameObject>() {aShelter}},
            {(_gameManager.Player, InventoryItems.Insulation), new List<GameObject> {aWarehouse}},
            {(_gameManager.Player, InventoryItems.Stone), new List<GameObject> {aWaveBreaker}},

            {(_gameManager.Player, InventoryItems.Wood), new List<GameObject>() {bObservationPoint}},
            {(_gameManager.Player, InventoryItems.Lenses), new List<GameObject>() {bObservatory}},
            {(_gameManager.Player, InventoryItems.Clay), new List<GameObject>() {bBrokenHomes, bRepairedHomes}},
            {(_gameManager.Player, InventoryItems.Gold), new List<GameObject>() {bTemple}},
            {(_gameManager.Player, InventoryItems.Steel), new List<GameObject>() {bShelter}},
            {(_gameManager.Player, InventoryItems.Insulation), new List<GameObject> {bWarehouse}},
            {(_gameManager.Player, InventoryItems.Stone), new List<GameObject> {bWaveBreaker}},

            {(_gameManager.Player, InventoryItems.Wood), new List<GameObject>() {cObservationPoint}},
            {(_gameManager.Player, InventoryItems.Lenses), new List<GameObject>() {cObservatory}},
            {(_gameManager.Player, InventoryItems.Clay), new List<GameObject>() {cBrokenHomes, cRepairedHomes}},
            {(_gameManager.Player, InventoryItems.Gold), new List<GameObject>() {cTemple}},
            {(_gameManager.Player, InventoryItems.Steel), new List<GameObject>() {cShelter}},
            {(_gameManager.Player, InventoryItems.Insulation), new List<GameObject> {cWarehouse}},
            {(_gameManager.Player, InventoryItems.Stone), new List<GameObject> {cWaveBreaker}},
        };

        _gameManager.Player.Inventory.InventoryUpdate += CheckIfBuildingPossible;
        _gameManager.Cpu1.Inventory.InventoryUpdate += CheckIfBuildingPossible;
        _gameManager.Cpu2.Inventory.InventoryUpdate += CheckIfBuildingPossible;
    }

    private void CheckIfBuildingPossible(object sender, EventArgs eventArgs)
    {
        // Check if any player has enough resources to build a building

        var resources = Enum.GetValues(typeof(InventoryItems));
        foreach (InventoryItems resource in resources)
        {
            if (_gameManager.Player.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Player, resource);
                AddBuildingPoints(_gameManager.Player, resource);
            }

            if (_gameManager.Cpu1.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Cpu1, resource);
                AddBuildingPoints(_gameManager.Cpu1, resource);
            }

            if (_gameManager.Cpu2.Inventory.GetInventoryAmount(resource) >= 10)
            {
                PlaceBuilding(_gameManager.Cpu2, resource);
                AddBuildingPoints(_gameManager.Cpu2, resource);
            }
        }
    }

    private void PlaceBuilding(Tribe tribe, InventoryItems resource)
    {
        var modelList = _buildableStructures[(tribe, resource)];

        if (modelList.Count == 1)
        {
            modelList[0].SetActive(true);
        }

        if (modelList.Count == 2)
        {
            modelList[0].SetActive(false);
            modelList[1].SetActive(true);
        }
    }

    private void AddBuildingPoints(Tribe tribe, InventoryItems resource)
    {
        _gameManager.Player.Points += _gameManager.Player.PointTable[(resource, tribe)];
        _gameManager.Cpu1.Points += _gameManager.Cpu1.PointTable[(resource, tribe)];
        _gameManager.Cpu2.Points += _gameManager.Cpu2.PointTable[(resource, tribe)];
    }
}