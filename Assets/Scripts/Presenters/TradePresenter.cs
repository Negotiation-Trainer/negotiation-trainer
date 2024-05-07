using System;
using System.Text;
using ModelLibrary;
using ModelLibrary.Exceptions;
using Newtonsoft.Json;
using ServiceLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Presenters
{
    public class TradePresenter: MonoBehaviour
    {
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
        [SerializeField] private GameObject accepted;
        [SerializeField] private GameObject rejected;
        

        private void Start()
        {
            _inputPresenter = GetComponent<InputPresenter>();
            _dialoguePresenter = GetComponent<DialoguePresenter>();
        }

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

        public void DiscardTradeOffer()
        {
            tradeOffer.SetActive(false);
            ClearOffer();
            _inputPresenter.ToggleTalkButton(true);
            _inputPresenter.ToggleNewOfferButton(true);
        }
        
         public void SignTradeOffer()
         {
             MakeTrade();
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
                    
                    Debug.Log("Trade Rejected");
                    
                    StartCoroutine(GameManager.httpClient.Reject(speakerStyle, _currentTrade, sb.ToString(),
                        RejectCallback));
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
            Debug.Log("Return Message" + returnMessage.Message);
            
            
            _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
            _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
                
            _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            
            Invoke(nameof(ClearOffer), 2);
            
            
            
            _dialoguePresenter.QueueMessages(_dialogueGenerationService.SplitTextToDialogueMessages(SplitMessageIntoChunks(returnMessage.Message), _target.Name));
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
            
            _dialoguePresenter.QueueMessages(_dialogueGenerationService.SplitTextToDialogueMessages(SplitMessageIntoChunks(returnMessage.Message), _target.Name));
        }

        private bool TradePossible(Trade trade, Tribe originator, Tribe target)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount && 
                   target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }

        private string SplitMessageIntoChunks(string message)
        {
            StringBuilder chunks = new StringBuilder();
            int currentIndex = 0;

            while (currentIndex < message.Length)
            {
                int chunkSize = 200;
                if (currentIndex + chunkSize > message.Length)
                {
                    chunkSize = message.Length - currentIndex;
                }
                else
                {
                    while (message[currentIndex + chunkSize] != ' ' && chunkSize > 0)
                    {
                        chunkSize--;
                    }
                }

                chunks.Append(message.Substring(currentIndex, chunkSize));
                currentIndex += chunkSize;

                // Add the separator if there is more message to process
                if (currentIndex < message.Length)
                {
                    chunks.Append("{nm}");
                }
            }

            return chunks.ToString();
        }
    }
}