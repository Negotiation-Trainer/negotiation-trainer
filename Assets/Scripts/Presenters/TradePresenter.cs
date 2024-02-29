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
            
            if (!TradePossible(trade, originator)) return;
            
            //Decision
            if (_algorithmService.Decide(trade, target))
            {
                Debug.Log("Trade accepted");
                originator.Inventory.RemoveFromInventory(trade.OfferedItem, trade.OfferedAmount);
                target.Inventory.AddToInventory(trade.OfferedItem, trade.OfferedAmount);
                
                originator.Inventory.AddToInventory(trade.RequestedItem, trade.RequestedAmount);
                target.Inventory.RemoveFromInventory(trade.RequestedItem, trade.RequestedAmount);
                
                //check if any of the players can build a building
                //if so, call the building presenter
                
                var itemsToCheck = new[] {trade.OfferedItem, trade.RequestedItem};
                foreach (var item in itemsToCheck)
                {
                    if (originator.Inventory.GetInventoryAmount(item) >= 10)
                    {
                        Debug.Log("Player can build a building");
                    }
                    if (target.Inventory.GetInventoryAmount(item) >= 10)
                    {
                        Debug.Log("CPU can build a building");
                    }
                }
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