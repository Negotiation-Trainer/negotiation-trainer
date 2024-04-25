using System;
using ModelLibrary;
using ModelLibrary.Exceptions;
using ServiceLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class TradePresenter : MonoBehaviour
    {
        private readonly AlgorithmService _algorithmService = new AlgorithmService();
        private InputPresenter _inputPresenter;

        private Trade _currentTrade;
        private Tribe _originator;
        private Tribe _target;

        [SerializeField] private GameObject tradeOffer;
        [SerializeField] private TMP_Text offerText;
        [SerializeField] private TMP_Text offerAmount;
        [SerializeField] private TMP_Text requestText;
        [SerializeField] private TMP_Text requestAmount;

        private void Start()
        {
            _inputPresenter = GetComponent<InputPresenter>();
            _algorithmService.AlgorithmDecision += OnAlgorithmDesicion;
        }

        public void ShowTradeOffer(Trade trade, Tribe originator, Tribe target)
        {
            _inputPresenter.ToggleNewOfferButton(false);
            if (!TradePossible(trade, originator, target)) return;

            _currentTrade = trade;
            _originator = originator;
            _target = target;

            if (_originator == GameManager.Instance.Player)
            {
                offerText.text = $"We, The {originator.Name} tribe, are offering";
                offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
                requestText.text = $"to the {target.Name} tribe, in exchange for:";
                requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
            }
            else
            {
                offerText.text = $"The {originator.Name} tribe, are offering";
                offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
                requestText.text = $"to us, the {target.Name} tribe, in exchange for:";
                requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
            }

            tradeOffer.SetActive(true);
        }

        public void DiscardTradeOffer()
        {
            tradeOffer.SetActive(false);
            ClearOffer();
            _inputPresenter.ToggleNewOfferButton(true);
        }

        public void SignTradeOffer()
        {
            tradeOffer.SetActive(false);
            if (_originator == GameManager.Instance.Player)
            {
                MakeTrade();
            }
            else
            {
                ProcessTrade();
                ClearOffer();
            }
            
            _inputPresenter.ToggleNewOfferButton(true);
        }

        private void ClearOffer()
        {
            _currentTrade = null;
            _originator = null;
            _target = null;
        }

        private void ProcessTrade()
        {
            _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
            _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);

            _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
        }

        private void MakeTrade()
        {
            if (_currentTrade == null || _originator == null || _target == null) return;
            try
            {
                _algorithmService.Decide(_currentTrade, _originator, _target);
                Debug.Log("Decide - no error");
            }
            catch (OfferDeclinedException e)
            {
                Debug.Log($"OFFER DECLINED {e.Message}");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            ClearOffer();
        }

        private void OnAlgorithmDesicion(object sender,
            AlgorithmService.AlgorithmDecisionEventArgs algorithmDecisionEventArgs)
        {
            if (algorithmDecisionEventArgs.tradeAccepted)
            {
                Debug.Log("ACCEPTED");
                ProcessTrade();
            }
            else if(!algorithmDecisionEventArgs.tradeAccepted && algorithmDecisionEventArgs.counterOffer != null)
            {
                Debug.Log("COUNTER");
                ShowTradeOffer(algorithmDecisionEventArgs.counterOffer, _target,_originator);
            }
            else
            {
                Debug.Log("DECLINE");
            }
        }

        private bool TradePossible(Trade trade, Tribe originator, Tribe target)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount &&
                   target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }
    }
}