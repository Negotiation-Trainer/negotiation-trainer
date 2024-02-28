using System;
using Models;

/*
 * This class is responsible for handling the algorithmic logic of the game.
 */
namespace LogicServices
{
    public class AlgorithmService
    {
        public bool Decide(Trade trade)
        {
            SetupAlgorithm();
            
            

            return true;
        }

        private void SetupAlgorithm()
        {
            //set up inventory
        }
        
        
        /* Decision Steps */
        private bool Algo_SelfBuild(Trade trade)
        {
            return trade.RequestedItem == trade.OfferedItem;
        }
        
    }
}