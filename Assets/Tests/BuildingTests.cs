using System;
using System.Collections.Generic;
using Enums;
using LogicServices;
using LogicServices.Algorithm;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class BuildingTests
    {
        [Test]
        public void CheckIfBuildingPossible_NotPossible_ReturnsNull()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");

            //when
            var result = buildingService.CheckIfBuildingPossible(tribe);

            //Then
            Assert.IsNull(result);
        }

        [Test]
        public void CheckIfBuildingPossible_IsPossible_ReturnsInventoryItem()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            tribe.Inventory.AddToInventory(InventoryItems.Gold, 10);

            //when
            var result = buildingService.CheckIfBuildingPossible(tribe);

            //Then
            Assert.AreEqual(InventoryItems.Gold, result);
        }

        [Test]
        public void AddBuildingPoints_BuildWood_AddPoints()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            Tribe tribe2 = new Tribe("tribe2");
            tribe.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tribe)] = 10,
                [(InventoryItems.Wood, tribe2)] = -5
            };
            tribe2.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, tribe)] = -5,
                [(InventoryItems.Wood, tribe2)] = 10
            };
            Tribe[] tribes = new[] {tribe, tribe2};

            //when
            buildingService.AddBuildingPoints(tribes,InventoryItems.Wood,tribe);

            //Then
            Assert.AreEqual(10, tribe.Points);
            Assert.AreEqual(-5, tribe2.Points);
        }
        
        [Test]
        public void RemoveBuildingResourcesFromInventory_BuildWood_RemovesResources()
        {
            //Given
            BuildingService buildingService = new BuildingService();
            Tribe tribe = new Tribe("tribe");
            tribe.Inventory.AddToInventory(InventoryItems.Wood,10);

            //when
            buildingService.RemoveBuildingResourcesFromInventory(tribe,InventoryItems.Wood);
            var result = tribe.Inventory.GetInventoryAmount(InventoryItems.Wood);

            //Then
            Assert.AreEqual(0, result);
        }
    }
}
