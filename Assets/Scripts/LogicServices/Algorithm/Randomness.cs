
using System;

namespace LogicServices.Algorithm
{
    public class Randomness
    {
        public float ChangeChance { get; set; }
        private readonly Random _random;

        public Randomness(Random random)
        {
            ChangeChance = 0.2f;
            _random = random;
        }

        public bool Calculate()
        {
            return (_random.NextDouble() < ChangeChance);
        }
    }
}