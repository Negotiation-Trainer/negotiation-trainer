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
        private readonly float _selfBuildRandomChance = 0.1f;
        private readonly float _buildEffectRandomChance = 0.1f;
        private readonly float _usefulnessRandomChance = 0.1f;
        private readonly float _tradeBalanceRandomChance = 0.1f;
        
        private readonly SelfBuild _selfBuild;
        private readonly Randomness _randomness;
        private readonly BuildEffect _buildEffect;
        private readonly Usefulness _usefulness;
        private readonly TradeBalance _tradeBalance;

        public AlgorithmService()
        {
            Random random = new Random();
            _selfBuild = new SelfBuild(random);
            _randomness = new Randomness(random);
            _buildEffect = new BuildEffect(random);
            _usefulness = new Usefulness(random);
            _tradeBalance = new TradeBalance();
        }

        public bool Decide(Trade trade,Tribe originator, Tribe targetCpu)
        {
            //Original Decisions
            bool selfBuildDecision = _selfBuild.Calculate(trade, targetCpu);
            bool buildEffectDecision = _buildEffect.Calculate(trade, targetCpu, originator);
            bool usefulnessDecision = _usefulness.Calculate(trade, targetCpu);
            bool tradeBalanceDecision = _tradeBalance.Calculate(trade);
            
            //Randomise Decisions
            selfBuildDecision = _randomness.Calculate(_selfBuildRandomChance) ? selfBuildDecision : !selfBuildDecision;
            buildEffectDecision = _randomness.Calculate(_buildEffectRandomChance) ? buildEffectDecision : !buildEffectDecision;
            usefulnessDecision = _randomness.Calculate(_usefulnessRandomChance) ? usefulnessDecision : !usefulnessDecision;
            tradeBalanceDecision = _randomness.Calculate(_tradeBalanceRandomChance) ? tradeBalanceDecision : !tradeBalanceDecision;
            
            return selfBuildDecision && buildEffectDecision && usefulnessDecision && tradeBalanceDecision;
        }
    }
}