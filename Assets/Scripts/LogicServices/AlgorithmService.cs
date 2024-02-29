using LogicServices.Algorithm;
using Models;
using UnityEngine;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace LogicServices
{
    public class AlgorithmService
    {
        private User _originator;
        private User _target;
        private Trade _trade;

        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;

        public AlgorithmService(int selfBuildBorder = 5, float randomChangeChance = 0.2f)
        {
            _selfBuild = new SelfBuild(selfBuildBorder);
            _randomness = new Randomness(randomChangeChance);
        }

        public bool Decide(Trade trade,User originator, User targetCpu)
        {
            //setup
            _originator = originator;
            _target = targetCpu;
            _trade = trade;
            
            if (_randomness.Calculate()) return !_selfBuild.Calculate(trade,targetCpu);
            return _selfBuild.Calculate(trade,targetCpu);
        }
    }
}