using Enums;
using LogicServices;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Presenters
{
    public class TradePresenter: MonoBehaviour
    {
        private readonly AlgorithmService _algorithmService = new AlgorithmService();
        
        [SerializeField] private InventoryItems debugRequestedItem = InventoryItems.Wood;
        [SerializeField] private int debugRequestedAmount = 2;
        [SerializeField] private InventoryItems debugOfferedItem = InventoryItems.Steel;
        [SerializeField] private int debugOfferedAmount = 2;
        public void PresentTrade()
        {
            var originator = GameManager.Instance.Player;

            // get from llm
            var trade = new Trade(debugRequestedItem, debugRequestedAmount, debugOfferedItem, debugOfferedAmount);
            var target = GameManager.Instance.Cpu2;
            
            if (!TradePossible(trade, originator, target)) return;
            
            //Decision
            if (_algorithmService.Decide(trade, originator, target))
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

        private bool TradePossible(Trade trade, Tribe originator, Tribe target)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount && 
                   target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }
    }
}