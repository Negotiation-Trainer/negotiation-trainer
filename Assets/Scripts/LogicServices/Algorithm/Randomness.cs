using UnityEngine;

namespace LogicServices.Algorithm
{
    public class Randomness
    {
        private readonly float _changeChance;

        public Randomness(float changeChance)
        {
            _changeChance = changeChance;
        }

        public bool Calculate()
        {
            return (Random.value < _changeChance);
        }
    }
}