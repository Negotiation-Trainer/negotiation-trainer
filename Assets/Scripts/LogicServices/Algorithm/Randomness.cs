
using System;

namespace LogicServices.Algorithm
{
    public class Randomness
    {
        private readonly float _changeChance;
        private readonly Random _random;

        public Randomness(float changeChance, Random random)
        {
            _changeChance = changeChance;
            _random = random;
        }

        public bool Calculate()
        {
            return (_random.NextDouble() < _changeChance);
        }
    }
}