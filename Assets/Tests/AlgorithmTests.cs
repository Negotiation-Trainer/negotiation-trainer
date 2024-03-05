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
            Randomness randomness = new Randomness(1);
            
            //Then
            Assert.IsTrue(randomness.Calculate());
        }
        
        [Test]
        public void CalculateRandomness_LowRandomness_ReturnsFalse()
        {
            //Given
            Randomness randomness = new Randomness(0);
            
            //Then
            Assert.IsFalse(randomness.Calculate());
        }
        
        [Test]
        public void CalculateSelfBuild_HigerThanBorder_ReturnsFalse()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(5);
            User user = new User();
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            user.Inventory.AddToInventory(InventoryItems.Wood,8);
            
            //Then
            Assert.IsFalse(selfBuild.Calculate(trade,user));
        }
        
        [Test]
        public void CalculateSelfBuild_LowerThanBorder_ReturnsTrue()
        {
            //Given
            SelfBuild selfBuild = new SelfBuild(5);
            User user = new User();
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            user.Inventory.AddToInventory(InventoryItems.Wood,1);
            
            //Then
            Assert.IsTrue(selfBuild.Calculate(trade,user));
        }
        
        [Test]
        public void Decide_GoodDeal_ReturnsTrue()
        {
            //Given
            AlgorithmService algorithmService = new AlgorithmService(5,0);
            User originator = new User();
            User target = new User();
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            target.Inventory.AddToInventory(InventoryItems.Wood,3);
            
            //Then
            Assert.IsTrue(algorithmService.Decide(trade, originator, target));
        }
        
        [Test]
        public void Decide_BadDeal_ReturnsFalse()
        {
            //Given
            AlgorithmService algorithmService = new AlgorithmService(5,0);
            User originator = new User();
            User target = new User();
            Trade trade = new Trade(InventoryItems.Wood,1,InventoryItems.Stone,1);
            
            //When
            target.Inventory.AddToInventory(InventoryItems.Wood,8);
            
            //Then
            Assert.IsFalse(algorithmService.Decide(trade, originator, target));
        }
    }
}
