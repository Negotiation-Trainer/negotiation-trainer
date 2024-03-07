using LogicServices.Algorithm;
using Models;
using UnityEngine;
using Random = System.Random;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace LogicServices
{
    public class AlgorithmService
    {
        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;
        private readonly BuildEffect _buildEffect;

        public AlgorithmService()
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(random);
            _randomness = new Randomness(random);
            _buildEffect = new BuildEffect(random);
        }

        public bool Decide(Trade trade,Tribe originator, Tribe targetCpu)
        {
            //Original Decisions
            bool selfBuildDecision = _selfBuild.Calculate(trade, targetCpu);
            bool buildEffectDecision = _buildEffect.Calculate(trade, targetCpu, originator);
            
            //Randomise Decisions
            selfBuildDecision = _randomness.Calculate(0.2f) ? selfBuildDecision : !selfBuildDecision;
            buildEffectDecision = _randomness.Calculate(0.1f) ? buildEffectDecision : !buildEffectDecision;
            
            Debug.Log($"SelfBuild: {selfBuildDecision} BuildEffect: {buildEffectDecision}");
            
            return selfBuildDecision && buildEffectDecision;
        }
    }
}