using System;
using System.Collections.Generic;
using Enums;
using LogicServices;
using LogicServices.Algorithm;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class AlgorithmTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateRandomness_HighRandomness_ReturnsTrue()
        {
            //Given
            Random random = new Random();
            Randomness randomness = new Randomness(random);
            randomness.ChangeChance = 1;
            
            //when
            var result = randomness.Calculate();
            
            //Then
            Assert.IsTrue(result);
        }
        
        [Test]
        public void CalculateRandomness_LowRandomness_ReturnsFalse()
        {
            //Given
            Random random = new Random();
            Randomness randomness = new Randomness(random);
            randomness.ChangeChance = 0;
            
            //when
            var result = randomness.Calculate();
            
            //Then
            Assert.IsFalse(result);
        }
        
        [Test]
        public void CalculateSelfBuild_HigerThanBorder_ReturnsFalse()
        {
            //Given
            Random random = new Random();
            SelfBuild selfBuild = new SelfBuild(random);
            selfBuild.SelfBuildThreshold = 5;
            Tribe tribe = new Tribe("test");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            tribe.Inventory.AddToInventory(InventoryItems.Wood,8);
            var result = selfBuild.Calculate(trade, tribe);
            
            //Then
            Assert.IsFalse(result);
        }
        
        [Test]
        public void CalculateSelfBuild_LowerThanBorder_ReturnsTrue()
        {
            //Given
            Random random = new Random();
            SelfBuild selfBuild = new SelfBuild(random);
            selfBuild.SelfBuildThreshold = 5;
            Tribe tribe = new Tribe("test");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            tribe.Inventory.AddToInventory(InventoryItems.Wood,1);
            var result = selfBuild.Calculate(trade, tribe);
            
            //Then
            Assert.IsTrue(result);
        }
        
        [Test]
        public void CalculateBuildEffect_GoodEffect_ReturnsTrue()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect();
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            target.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, target)] = 10,
                [(InventoryItems.Wood, originator)] = 5
            };
            
            //Then
            Assert.IsTrue(buildEffect.Calculate(trade,target,originator));
        }
        
        [Test]
        public void CalculateBuildEffect_NoEffect_ReturnsTrue()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect();
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            target.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, target)] = 10,
                [(InventoryItems.Wood, originator)] = 0
            };
            
            //Then
            Assert.IsTrue(buildEffect.Calculate(trade,target,originator));
        }
        
        [Test]
        public void CalculateBuildEffect_BadEffect_ReturnsFalse()
        {
            //Given
            BuildEffect buildEffect = new BuildEffect();
            Tribe originator = new Tribe("originator");
            Tribe target = new Tribe("target");
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            target.PointTable = new Dictionary<(InventoryItems, Tribe), int>
            {
                [(InventoryItems.Wood, target)] = 10,
                [(InventoryItems.Wood, originator)] = -5
            };
            
            //Then
            Assert.IsFalse(buildEffect.Calculate(trade,target,originator));
        }
    }
}
