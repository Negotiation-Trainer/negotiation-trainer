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

        public AlgorithmService(int selfBuildThreshold = 5, float randomChangeChance = 0.2f)
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(selfBuildThreshold, random);
            _randomness = new Randomness(randomChangeChance, random);
        }

        public bool Decide(Trade trade,User originator, User targetCpu)
        {
            if (_randomness.Calculate()) return !_selfBuild.Calculate(trade,targetCpu);
            return _selfBuild.Calculate(trade,targetCpu);
        }
    }
}