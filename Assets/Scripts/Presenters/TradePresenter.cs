using Enums;
using LogicServices;
using Models;
using UnityEngine;

namespace Presenters
{
    public class TradePresenter: MonoBehaviour
    {
        private readonly AlgorithmService _algorithmService = new AlgorithmService();
        public void PresentTrade()
        {
            var originator = GameManager.Instance.Player;

            // get from llm
            var trade = new Trade(InventoryItems.Wood, 1, InventoryItems.Stone, 1);
            var target = GameManager.Instance.Cpu2;
            
            if (!TradePossible(trade, originator)) return;
            
            //Decision
            if (_algorithmService.Decide(trade, target))
            {
                Debug.Log("Trade accepted");
                originator.Inventory.RemoveFromInventory(trade.OfferedItem, trade.OfferedAmount);
                target.Inventory.AddToInventory(trade.OfferedItem, trade.OfferedAmount);
                
                originator.Inventory.AddToInventory(trade.RequestedItem, trade.RequestedAmount);
                target.Inventory.RemoveFromInventory(trade.RequestedItem, trade.RequestedAmount);
            }
            else
            {
                Debug.Log("Trade Refused");
            }
        }

        private bool TradePossible(Trade trade, User originator)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount;
        }
    }
}