using System;
using LogicServices.Algorithm;
using Models;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace LogicServices
{
    public class AlgorithmService
    {
        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;

        public AlgorithmService()
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(random);
            _randomness = new Randomness(random);
        }

        public bool Decide(Trade trade,Tribe originator, Tribe targetCpu)
        {
            if (_randomness.Calculate()) return !_selfBuild.Calculate(trade,targetCpu);
            return _selfBuild.Calculate(trade,targetCpu);
        }
    }
}