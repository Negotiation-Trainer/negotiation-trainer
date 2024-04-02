using Enums;
using LogicServices;
using Models;
using TMPro;
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

        [SerializeField] private GameObject tradeOffer;
        [SerializeField] private TMP_Text offerText;
        [SerializeField] private TMP_Text offerAmount;
        [SerializeField] private TMP_Text requestText;
        [SerializeField] private TMP_Text requestAmount;

        public void ShowTradeOffer(Trade trade, Tribe originator, Tribe target)
        {
            tradeOffer.SetActive(true);
            offerText.text = $"We, The {originator.Name} tribe, are offering";
            offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
            requestText.text = $"to the {target.Name} tribe, in exchange for:";
            requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
        }

        public void DiscardTradeOffer()
        {
            tradeOffer.SetActive(false);
        }
        
         public void SignTradeOffer()
         {
             tradeOffer.SetActive(false);
         }
        
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