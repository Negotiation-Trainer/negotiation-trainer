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

        private readonly SelfBuild _selfBuild = new SelfBuild(5);
        private readonly Randomness _randomness = new Randomness(0.2f);

        public bool Decide(Trade trade, User targetCpu)
        {
            //setup
            _originator = GameManager.Instance.Player;
            _target = targetCpu;
            _trade = trade;
            
            if (_randomness.Calculate()) return !_selfBuild.Calculate(trade,targetCpu);
            return _selfBuild.Calculate(trade,targetCpu);
        }
    }
}