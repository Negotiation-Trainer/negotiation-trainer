
using System;

namespace LogicServices.Algorithm
{
    public class Randomness
    {
        private readonly Random _random;

        public Randomness(Random random)
        {
            _random = random;
        }

        public bool Calculate(float changeChance)
        {
            return (_random.NextDouble() < changeChance);
        }
    }
}