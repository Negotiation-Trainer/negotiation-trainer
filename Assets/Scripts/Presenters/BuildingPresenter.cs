using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingPresenter : MonoBehaviour
{
    /*
     * This class is responsible for handling the construction of buildings.
     *
     * It should be able to:
     * - Place a building on the map at the right position
     * - Keep track of the buildings that are placed
     */
    
    private Dictionary<InventoryItems, Vector3> _instantiateLocations = new()
    {
        {InventoryItems.Wood, new Vector3(10, 0, 0)},
        {InventoryItems.Stone, new Vector3(0, 0, 0)},
        {InventoryItems.Clay, new Vector3(10, 0, 10)},
    };
    [SerializeField] private List<GameObject> buildings = new();
    
    [SerializeField]
    private GameObject buildingPrefab;
    
    private void Start()
    {
        Debug.Log("BuildingPresenter started");
    }
    
    private void checkIfBuildingPossible()
    {
        // Check if the player has enough resources to build a building
    }

    public void PlaceBuilding(InventoryItems materialType)
    {
        if (_instantiateLocations.TryGetValue(materialType, out var instantiateLocation))
        {
            var building = Instantiate(buildingPrefab, instantiateLocation, Quaternion.identity);
            buildings.Add(building);
        }
        else
        {
            Debug.LogError("No location found for material type: " + materialType);
        }
    }
}