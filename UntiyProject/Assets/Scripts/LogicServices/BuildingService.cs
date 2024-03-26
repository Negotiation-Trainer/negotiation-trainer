using System;
using System.Collections.Generic;
using Enums;
using Models;

namespace LogicServices
{
    public class BuildingService
    {
        private const int BuildingResourceCost = 10;
        
        public InventoryItems? CheckIfBuildingPossible(Tribe tribe)
        {
            var resources = Enum.GetValues(typeof(InventoryItems));
            foreach (InventoryItems resource in resources)
            {
                if (tribe.Inventory.GetInventoryAmount(resource) >= BuildingResourceCost)
                {
                    return resource;
                }
            }
            return null;
        }

        public void RemoveBuildingResourcesFromInventory(Tribe builder, InventoryItems resource)
        {
            builder.Inventory.RemoveFromInventory(resource, BuildingResourceCost);
        }
        
        public void AddBuildingPoints(Tribe[] tribes, InventoryItems resource, Tribe builder)
        {
            foreach (var tribe in tribes)
            {
                tribe.Points += tribe.PointTable[(resource, builder)];
            }
        }
    }
}