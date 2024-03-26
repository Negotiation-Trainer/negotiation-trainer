using System;
using Models;

namespace LogicServices.Algorithm
{
    public class BuildEffect
    {
        private readonly Random _random;
        
        public BuildEffect(Random random)
        {
            _random = random;
        }
        
        ///Checks if letting the trade originator build the requested building is beneficial
        ///<returns>
        ///true if gain points.
        ///random if neutral.
        ///false if lose points 
        ///</returns>
        public bool Calculate(Trade trade, Tribe target, Tribe originator)
        {
            var points = target.PointTable[(trade.RequestedItem, originator)];
            return points switch
            {
                5 => true,
                0 => _random.NextDouble() > 0.5f,
                _ => false
            };
        }
    }
}