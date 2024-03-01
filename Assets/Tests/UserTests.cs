using System;
using System.Collections.Generic;
using Enums;
using LogicServices;
using LogicServices.Algorithm;
using Models;
using NUnit.Framework;

namespace Tests
{
    public class UserTests
    {
        [Test]
        public void PointTable_GetPoints_ReturnsPoints()
        {
            //Given
            User user = new User();
            User user2 = new User();
            user.PointTable = new Dictionary<(InventoryItems, User), int>
            {
                [(InventoryItems.Wood, user)] = 10,
                [(InventoryItems.Wood, user2)] = -5
            };
            
            //Then
            Assert.AreEqual(10,user.PointTable[(InventoryItems.Wood,user)]);
        }
    }
}
