using System;
using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;

namespace Presenters
{
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
        private Dictionary<Tribe, List<GameObject>> _buildStructures;

        private void Start()
        {
            _gameManager = GameManager.Instance;

            _buildableStructures = new()
            {
                {(_gameManager.Player, InventoryItems.Wood), new List<GameObject>() {aObservationPoint}},
                {(_gameManager.Player, InventoryItems.Lenses), new List<GameObject>() {aObservatory}},
                {(_gameManager.Player, InventoryItems.Clay), new List<GameObject>() {aBrokenHomes, aRepairedHomes}},
                {(_gameManager.Player, InventoryItems.Gold), new List<GameObject>() {aTemple}},
                {(_gameManager.Player, InventoryItems.Steel), new List<GameObject>() {aShelter}},
                {(_gameManager.Player, InventoryItems.Insulation), new List<GameObject> {aWarehouse}},
                {(_gameManager.Player, InventoryItems.Stone), new List<GameObject> {aWaveBreaker}},

                {(_gameManager.Cpu1, InventoryItems.Wood), new List<GameObject>() {bObservationPoint}},
                {(_gameManager.Cpu1, InventoryItems.Lenses), new List<GameObject>() {bObservatory}},
                {(_gameManager.Cpu1, InventoryItems.Clay), new List<GameObject>() {bBrokenHomes, bRepairedHomes}},
                {(_gameManager.Cpu1, InventoryItems.Gold), new List<GameObject>() {bTemple}},
                {(_gameManager.Cpu1, InventoryItems.Steel), new List<GameObject>() {bShelter}},
                {(_gameManager.Cpu1, InventoryItems.Insulation), new List<GameObject> {bWarehouse}},
                {(_gameManager.Cpu1, InventoryItems.Stone), new List<GameObject> {bWaveBreaker}},

                {(_gameManager.Cpu2, InventoryItems.Wood), new List<GameObject>() {cObservationPoint}},
                {(_gameManager.Cpu2, InventoryItems.Lenses), new List<GameObject>() {cObservatory}},
                {(_gameManager.Cpu2, InventoryItems.Clay), new List<GameObject>() {cBrokenHomes, cRepairedHomes}},
                {(_gameManager.Cpu2, InventoryItems.Gold), new List<GameObject>() {cTemple}},
                {(_gameManager.Cpu2, InventoryItems.Steel), new List<GameObject>() {cShelter}},
                {(_gameManager.Cpu2, InventoryItems.Insulation), new List<GameObject> {cWarehouse}},
                {(_gameManager.Cpu2, InventoryItems.Stone), new List<GameObject> {cWaveBreaker}},
            };

            _buildStructures = new Dictionary<Tribe, List<GameObject>>
            {
                [_gameManager.Player] = new List<GameObject>(),
                [_gameManager.Cpu1] = new List<GameObject>(),
                [_gameManager.Cpu2] = new List<GameObject>()
            };

            _gameManager.Player.Inventory.InventoryUpdate += InventoryUpdateEventHandler;
            _gameManager.Cpu1.Inventory.InventoryUpdate += InventoryUpdateEventHandler;
            _gameManager.Cpu2.Inventory.InventoryUpdate += InventoryUpdateEventHandler;
        }
    
        private void InventoryUpdateEventHandler(object sender, EventArgs eventArgs)
        {
            CheckIfBuildingPossible(_gameManager.Player);
            CheckIfBuildingPossible(_gameManager.Cpu1);
            CheckIfBuildingPossible(_gameManager.Cpu2);
        }

        private void CheckIfBuildingPossible(Tribe tribe)
        {
            var resources = Enum.GetValues(typeof(InventoryItems));
            foreach (InventoryItems resource in resources)
            {
                if (tribe.Inventory.GetInventoryAmount(resource) >= 10)
                {
                    PlaceBuilding(tribe, resource);
                    AddBuildingPoints(tribe, resource);
                    tribe.Inventory.RemoveFromInventory(resource, 10);
                }
            }
        }

        private void PlaceBuilding(Tribe tribe, InventoryItems resource)
        {
            var modelList = _buildableStructures[(tribe, resource)];

            if (modelList.Count == 1)
            {
                _buildStructures[tribe].Add(modelList[0]);
                modelList[0].SetActive(true);
            }

            if (modelList.Count == 2)
            {
                _buildStructures[tribe].Add(modelList[1]);
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
}