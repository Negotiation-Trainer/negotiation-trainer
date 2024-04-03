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
        
        private Trade _currentTrade;
        private Tribe _originator;
        private Tribe _target;

        [SerializeField] private GameObject tradeOffer;
        [SerializeField] private TMP_Text offerText;
        [SerializeField] private TMP_Text offerAmount;
        [SerializeField] private TMP_Text requestText;
        [SerializeField] private TMP_Text requestAmount;

        public void ShowTradeOffer(Trade trade, Tribe originator, Tribe target)
        {
            if(!TradePossible(trade,originator,target)) return;

            _currentTrade = trade;
            _originator = originator;
            _target = target;
            
            offerText.text = $"We, The {originator.Name} tribe, are offering";
            offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
            requestText.text = $"to the {target.Name} tribe, in exchange for:";
            requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
            
            tradeOffer.SetActive(true);
        }

        public void DiscardTradeOffer()
        {
            tradeOffer.SetActive(false);
            ClearOffer();
        }
        
         public void SignTradeOffer()
         {
             tradeOffer.SetActive(false);
             MakeTrade();
         }

         private void ClearOffer()
         {
             _currentTrade = null;
             _originator = null;
             _target = null;
         }
        
        private void MakeTrade()
        {
            if (_currentTrade == null || _originator == null || _target == null) return;
            if (_algorithmService.Decide(_currentTrade, _originator, _target))
            {
                Debug.Log("Trade accepted");
                _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
                _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
                
                _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
                _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            }
            else
            {
                Debug.Log("Trade Refused");
            }
            ClearOffer();
        }

        private bool TradePossible(Trade trade, Tribe originator, Tribe target)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount && 
                   target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }
    }
}