using Enums;
using LogicServices;
using Models;
using UnityEngine;

namespace Presenters
{
    public class TradePresenter: MonoBehaviour
    {
        public void PresentTrade()
        {
            var trade = new Trade(InventoryItems.Wood, 1, InventoryItems.Stone, 1);

            var algorithmService = new AlgorithmService();
            
            Debug.Log(algorithmService.Decide(trade) ? "Trade accepted" : "Trade rejected");
        }
    }
}