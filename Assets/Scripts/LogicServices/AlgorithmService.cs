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
        private readonly BuildEffect _buildEffect;

        public AlgorithmService(int selfBuildBorder = 5, float randomChangeChance = 0.2f)
        {
            _selfBuild = new SelfBuild(selfBuildBorder);
            _randomness = new Randomness(randomChangeChance);
            _buildEffect = new BuildEffect();
        }

        public bool Decide(Trade trade,User originator, User targetCpu)
        {
            //setup
            _originator = originator;
            _target = targetCpu;
            _trade = trade;
            
            //Decisions
            bool randomDecision = _randomness.Calculate();
            bool selfBuildDecision = _selfBuild.Calculate(trade, targetCpu);
            bool buildEffectDecision = _buildEffect.Calculate(trade, targetCpu, originator);
            
            Debug.Log($"Random: {randomDecision} SelfBuild: {selfBuildDecision} BuildEffect: {buildEffectDecision}");

            if (randomDecision) return !(selfBuildDecision && buildEffectDecision);
            return selfBuildDecision && buildEffectDecision;
        }
    }
}