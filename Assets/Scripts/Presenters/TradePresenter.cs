using System;
using System.Linq.Expressions;
using System.Text;
using ModelLibrary;
using ModelLibrary.Exceptions;
using Newtonsoft.Json;
using ServiceLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class TradePresenter : MonoBehaviour
    {
        public bool isTrading = false;
        
        private readonly AlgorithmService _algorithmService = new();
        private readonly DialogueGenerationService _dialogueGenerationService = new();
        private InputPresenter _inputPresenter;
        private DialoguePresenter _dialoguePresenter;
        
        private Trade _currentTrade;
        private Tribe _originator;
        private Tribe _target;

        [SerializeField] private GameObject tradeOffer;
        [SerializeField] private TMP_Text offerText;
        [SerializeField] private TMP_Text offerAmount;
        [SerializeField] private TMP_Text requestText;
        [SerializeField] private TMP_Text requestAmount;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private GameObject accepted;
        [SerializeField] private GameObject rejected;

        private void Start()
        {
            _inputPresenter = GetComponent<InputPresenter>();
            _dialoguePresenter = GetComponent<DialoguePresenter>();
            _algorithmService.AlgorithmDecision += OnAlgorithmDecision;
        }
        
        ///Show error text at bottom of trade offer.
        private void ShowError(string error)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = error;
        }

        ///Hide error text on trade offer. 
        private void HideError()
        {
            errorText.gameObject.SetActive(false);
        }

        /// <summary>
        /// Show trade offer on the screen.
        /// </summary>
        /// <param name="trade">The trade offer</param>
        /// <param name="originator">The tribe that made the offer</param>
        /// <param name="target">The tribe the offer targets</param>
        public void ShowTradeOffer(Trade trade, Tribe originator, Tribe target)
        {
            _inputPresenter.ToggleNewOfferButton(false);
            _inputPresenter.ToggleTalkButton(false);
            if(!TradePossible(trade,originator,target)) return;

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

        /// <summary>
        /// Decline trade offer. Hides UI and clears the offer.
        /// </summary>
        public void DiscardTradeOffer()
        {
            tradeOffer.SetActive(false);
            accepted.SetActive(false);
            rejected.SetActive(false);
            ClearOffer();
            _inputPresenter.ToggleTalkButton(true);
            _inputPresenter.ToggleNewOfferButton(true);
        }
        
         private void ClearOffer()
         {
             _currentTrade = null;
             _originator = null;
             _target = null;
             tradeOffer.SetActive(false);
             accepted.SetActive(false);
             rejected.SetActive(false);
             _inputPresenter.ToggleNewOfferButton(true); 
             _inputPresenter.ToggleTalkButton(true);
         }
         
        private void MakeTrade()
        {
            Debug.Log("Make a trade was called.");
            string speakerStyle = "lunatic";
            if (_currentTrade == null || _originator == null || _target == null) return;
            
            _algorithmService.AlgorithmDecision += (sender, args) =>
            {
                if (args.issuesWithTrade.Count > 0) // When issues with the trade are found, the trade will be rejected
                {
                    StringBuilder sb = new StringBuilder();
                    
                    foreach (var t in args.issuesWithTrade)
                    {
                        sb.Append($"{t.Message} +");
                    }

                    if (!isTrading)
                    {
                        isTrading = true;
                        Debug.Log("Trade Rejected");

                        StartCoroutine(GameManager.httpClient.Reject(speakerStyle, _currentTrade, sb.ToString(),
                            RejectCallback));
                    }
                }
                else
                {
                    accepted.SetActive(true);
                    Debug.Log("Trade accepted");

                    StartCoroutine(GameManager.httpClient.Accept(speakerStyle, _currentTrade, "none", AcceptCallback));
                }
            };
            
            _algorithmService.Decide(_currentTrade, _originator, _target);
        }

        private void AcceptCallback(string response)
        {
            //TODO: AI Service fix
            ChatMessage returnMessage = JsonConvert.DeserializeObject<ChatMessage>(response);
            Debug.Log("Return Message: " + returnMessage.Message);
            
            
            _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
            _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
                
            _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            
            Invoke(nameof(ClearOffer), 2);
            
            DisableButtons();
            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.QueueMessages(_dialogueGenerationService.SplitTextToDialogueMessages(returnMessage.Message, _target.Name));
            _dialoguePresenter.ShowNextMessage();
        }

        private void RejectCallback(string response)
        {
            Debug.Log("Rejected: " + response);
            
            //TODO: AI Service fix
            ChatMessage returnMessage = JsonConvert.DeserializeObject<ChatMessage>(response);
            Debug.Log("Return Message" + returnMessage.Message);
            
            rejected.SetActive(true);
            Debug.Log("Trade Refused");
            Invoke(nameof(ClearOffer), 2);
            
            DisableButtons();
            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.QueueMessages(_dialogueGenerationService.SplitTextToDialogueMessages(returnMessage.Message, _target.Name));
            _dialoguePresenter.ShowNextMessage();

        /// <summary>
        /// Accept the current trade offer.
        /// </summary>
        public void SignTradeOffer()
        {
            if (_originator == GameManager.Instance.Player)
            {
                if (TradePossible(_currentTrade, _originator, _target))
                {
                    MakeTrade();
                }
                else
                {
                    ShowError("you don't have enough resources");
                }
            }
            else
            {
                if (TradePossible(_currentTrade, _originator, _target))
                {
                    accepted.SetActive(true);
                    ProcessTrade();
                    Invoke(nameof(DiscardTradeOffer), 2);
                    return;
                }
                ShowError("you don't have enough resources");
            }
        }

        /// Clear the current offer and hide error.
        private void ClearOffer()
        {
            _currentTrade = null;
            _originator = null;
            _target = null;
            HideError();
        }
        
        /// Handles the inventory changes after the trade offer has been accepted
        private void ProcessTrade()
        {
            _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
            _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);

            _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
        }

        /// Make algorithm process the trade offer.
        private void MakeTrade()
        {
            if (_currentTrade == null || _originator == null || _target == null) return;
            try
            {
                _algorithmService.Decide(_currentTrade, _originator, _target);
            }
            catch (OfferDeclinedException e)
            {
                Debug.Log($"OFFER DECLINED {e.Message}");
            }
        }

        /// Handle the algorithm decision event. Either accepts the offer, shows a counter offer or declines the offer.
        private void OnAlgorithmDecision(object sender,
            AlgorithmService.AlgorithmDecisionEventArgs algorithmDecisionEventArgs)
        {
            if (algorithmDecisionEventArgs.tradeAccepted)
            {
                //Accept the players offer.
                accepted.SetActive(true);
                ProcessTrade();
                Invoke(nameof(DiscardTradeOffer), 2);
            }
            else if(!algorithmDecisionEventArgs.tradeAccepted && algorithmDecisionEventArgs.counterOffer != null && algorithmDecisionEventArgs.counterOffer == _currentTrade)
            {
                //TEMPORARY decline when counter is same as original offer. 
                rejected.SetActive(true);
                Invoke(nameof(DiscardTradeOffer), 2);
            }
            else if(!algorithmDecisionEventArgs.tradeAccepted && algorithmDecisionEventArgs.counterOffer != null)
            {
                //Present counter offer to player
                ShowTradeOffer(algorithmDecisionEventArgs.counterOffer, _target,_originator);
            }
            else
            {
                //Offer should be declined by the AI.
                rejected.SetActive(true);
                Invoke(nameof(DiscardTradeOffer), 2);
            }
        }

        /// Check if the participants of the deal have enough resources.
        private bool TradePossible(Trade trade, Tribe originator, Tribe target)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount &&
                   target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }

        private void EnableButtons(object sender, EventArgs args)
        {
            _inputPresenter.ToggleNewOfferButton(true);
            _inputPresenter.ToggleTalkButton(true);
        }

        private void DisableButtons()
        {
            _inputPresenter.ToggleNewOfferButton(false);
            _inputPresenter.ToggleTalkButton(false);
        }
    }
}