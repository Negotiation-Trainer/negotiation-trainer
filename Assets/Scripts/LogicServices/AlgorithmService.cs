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


        public bool Decide(Trade trade, User targetCpu)
        {
            //setup
            _originator = GameManager.Instance.Player;
            _target = targetCpu;
            _trade = trade;
            
            return Algo_SelfBuild();
        }

        /* Decision Steps */
        private bool Algo_SelfBuild()
        {
            if (_target.Inventory.GetInventoryAmount(_trade.RequestedItem) < 5) return true;
            if (_target.Inventory.GetInventoryAmount(_trade.RequestedItem) > 5) return false;
            return (Random.value > 0.5f);
        }
    }
}