using LogicServices.Algorithm;
using Models;
using System;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace LogicServices
{
    public class AlgorithmService
    {
        private float _selfBuildRandomChance = 0.1f;
        private float _buildEffectRandomChance = 0.1f;
        
        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;
        private readonly BuildEffect _buildEffect;
        private readonly Usefulness _usefulness;

        public AlgorithmService()
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(random);
            _randomness = new Randomness(random);
            _buildEffect = new BuildEffect(random);
            _usefulness = new Usefulness(random);
        }

        public bool Decide(Trade trade,Tribe originator, Tribe targetCpu)
        {
            //Original Decisions
            bool selfBuildDecision = _selfBuild.Calculate(trade, targetCpu);
            bool buildEffectDecision = _buildEffect.Calculate(trade, targetCpu, originator);
            bool usefulnessDecision = _usefulness.Calculate(trade, targetCpu);
            
            //Randomise Decisions
            selfBuildDecision = _randomness.Calculate(_selfBuildRandomChance) ? selfBuildDecision : !selfBuildDecision;
            buildEffectDecision = _randomness.Calculate(_buildEffectRandomChance) ? buildEffectDecision : !buildEffectDecision;
            
            return selfBuildDecision && buildEffectDecision && usefulnessDecision;
        }
    }
}