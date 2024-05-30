using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelLibrary;
using Newtonsoft.Json;
using ServiceLibrary;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Presenters
{
    public class TradePresenter : MonoBehaviour
    {
        private readonly AlgorithmService _algorithmService = new();
        private InputPresenter _inputPresenter;
        private DialoguePresenter _dialoguePresenter;
        
        private readonly Queue<Tribe> _turnCycle = new();
        private Tribe _currentTribe;
        private Trade _currentTrade;
        private Tribe _originator;
        private Tribe _target;
        private List<Trade> _tradeOffers = new();

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

            _dialoguePresenter.DialogueFinished += OnTTSFinished;
            
            // Create list of available tribes
            _turnCycle.Enqueue(GameManager.Instance.Player);
            _turnCycle.Enqueue(GameManager.Instance.Cpu1);
            _turnCycle.Enqueue(GameManager.Instance.Cpu2);
            
            GoToNextTribe();
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
        
        // Get the next tribe in the turn cycle.
        private void GoToNextTribe()
        {
            _currentTribe = _turnCycle.Dequeue();
            _turnCycle.Enqueue(_currentTribe);


            if (_currentTribe == GameManager.Instance.Player) return; // When the player is the current tribe, do nothing.
            
            StartCPUTradingProcess();
        }

        // Start the trading process for the CPU.
        private void StartCPUTradingProcess()
        {
            var availableTradeTargets = _turnCycle.ToArray().Where(tribe => tribe != _currentTribe);
            var tradeTargets = availableTradeTargets.ToList();
            var target = tradeTargets.ElementAt(new Random().Next(tradeTargets.Count));
            CreateNewTrade(_currentTribe, target);
        }

        private void OnTTSFinished(object sender, EventArgs eventArgs)
        {
            if (GameManager.Instance.State != GameManager.GameState.Trade) return;
            if (_currentTribe == GameManager.Instance.Player) return;
            if (_dialoguePresenter.MessagesRemaining() == 0)
            {
                if (_target != GameManager.Instance.Player && _target != null)
                {
                    DecideOnTrade();
                }
            }
            
        }

        /// <summary>
        /// Show trade offer on the screen.
        /// </summary>
        /// <param name="trade">The trade offer</param>
        /// <param name="originator">The tribe that made the offer</param>
        /// <param name="target">The tribe the offer targets</param>
        public void ShowTradeOffer(Trade trade, Tribe originator, Tribe target)
        {
            DisableButtons();
            
            if (originator == GameManager.Instance.Player)
            {
                if (!TradePossibleForOriginator(trade,originator)) return;
                offerText.text = $"We, The {originator.Name} tribe, are offering";
                offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
                requestText.text = $"to the {target.Name} tribe, in exchange for:";
                requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
            }
            else
            {
                if (!TradePossibleForTarget(trade, target)) return;
                offerText.text = $"The {originator.Name} tribe, are offering";
                offerAmount.text = $"{trade.OfferedAmount} {trade.OfferedItem}";
                requestText.text = $"to us, the {target.Name} tribe, in exchange for:";
                requestAmount.text = $"{trade.RequestedAmount} {trade.RequestedItem}";
            }
            
            _currentTrade = trade;
            _originator = originator;
            _target = target;

            tradeOffer.SetActive(true);
        }

        /// <summary>
        /// Clear current offer and deactivate the UI
        /// </summary>
        public void DiscardTradeOffer()
        {
            _currentTrade = null;
            _originator = null;
            _target = null;
            HideError();
            tradeOffer.SetActive(false);
            accepted.SetActive(false);
            rejected.SetActive(false);
            _inputPresenter.ToggleNewOfferButton(true);
            _inputPresenter.ToggleTalkButton(true);
        }
        
        private void DecideOnTrade()
        {
            Debug.Log("Make a trade was called.");
            if (_currentTrade == null || _originator == null || _target == null) return;
            _algorithmService.Decide(_currentTrade, _originator, _target);
        }

        public void CreateNewTrade(Tribe originator, Tribe target)
        {
            const int maxIterations = 1000; // Prevent the while loop from executing indefinitely
            int currentIteration = 0;
        
            Debug.Log("Create new trade was called.");
            Trade proposedTrade = _algorithmService.CreateNewTrade(originator, target);
            Debug.Log($"Trade: {proposedTrade.OfferedAmount} {proposedTrade.OfferedItem} for {proposedTrade.RequestedAmount} {proposedTrade.RequestedItem}");
        
            while (_tradeOffers.Contains(proposedTrade) && currentIteration < maxIterations)
            {
                proposedTrade = _algorithmService.CreateNewTrade(originator, target);
                currentIteration++;
            }
        
            if (currentIteration == maxIterations)
            {
                // Handle the case where a unique trade could not be found
                var errorMessage = "Could not find a unique trade after " + maxIterations + " attempts.";
                Debug.Log(errorMessage);
                ShowError(errorMessage);
                return;
            }
        
            _tradeOffers.Add(proposedTrade);
            
            StartCoroutine(GameManager.httpClient.ConvertToChat("lunatic", proposedTrade, ConvertTradeToChatCallback));
            
            ShowTradeOffer(proposedTrade, originator, target);
        }

        private void AcceptCallback(string response)
        {
            ChatMessage returnMessage = JsonConvert.DeserializeObject<ChatMessage>(response);
            
            ProcessInventoryChanges();
            accepted.SetActive(true);
            Invoke(nameof(DiscardTradeOffer), 2);
            
            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.EnqueueAndShowDialogueString(returnMessage.Message, _target.Name);
        }

        private void RejectCallback(string response)
        {
            ChatMessage returnMessage = JsonConvert.DeserializeObject<ChatMessage>(response);

            rejected.SetActive(true);
            Invoke(nameof(DiscardTradeOffer), 2);

            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.EnqueueAndShowDialogueString(returnMessage.Message, _target.Name);
        }

        private void CounterOfferCallback(string response)
        {
            ChatMessage counterOfferMessage = JsonConvert.DeserializeObject<ChatMessage>(response);
            
            Invoke(nameof(DiscardTradeOffer), 2);
            
            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.EnqueueAndShowDialogueString(counterOfferMessage.Message, _target.Name);
        }

        private void ConvertTradeToChatCallback(string response)
        {
            ChatMessage TradeMessage = JsonConvert.DeserializeObject<ChatMessage>(response);
            
            _dialoguePresenter.DialogueFinished += EnableButtons;
            _dialoguePresenter.EnqueueAndShowDialogueString(TradeMessage.Message, _target.Name);
        }

        /// <summary>
        /// Accept the current trade offer.
        /// </summary>
        public void SignTradeOffer()
        {
            if (_originator == GameManager.Instance.Player)
            {
                if (TradePossibleForOriginator(_currentTrade, _originator))
                {
                    DecideOnTrade();
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
                    _tradeOffers.Remove(_currentTrade); // Remove the trade from the list of trade offers because it was accepted.
                    ProcessInventoryChanges();
                    Invoke(nameof(DiscardTradeOffer), 2);
                    GoToNextTribe();
                    return;
                }

                ShowError("you don't have enough resources");
            }
        }

        /// Handles the inventory changes after the trade offer has been accepted
        private void ProcessInventoryChanges()
        {
            _originator.Inventory.RemoveFromInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);
            _target.Inventory.AddToInventory(_currentTrade.OfferedItem, _currentTrade.OfferedAmount);

            _originator.Inventory.AddToInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
            _target.Inventory.RemoveFromInventory(_currentTrade.RequestedItem, _currentTrade.RequestedAmount);
        }
            
        /// Handle the algorithm decision event. Either accepts the offer, shows a counter offer or declines the offer.
        private void OnAlgorithmDecision(object sender,
            AlgorithmService.AlgorithmDecisionEventArgs algorithmDecisionEventArgs)
        {
            string speakerStyle = "lunatic"; //TODO: Move this to either inside the Tribe object or make a map available here to map tribe to speaker style.
            StringBuilder reasonList = new StringBuilder();

            foreach (var offerDeclinedException in algorithmDecisionEventArgs.issuesWithTrade)
            {
                reasonList.Append($"{offerDeclinedException.Message} +");
            }
                
            switch (algorithmDecisionEventArgs.tradeAccepted) 
            {
                case true:
                    //Accept the players offer.
                    StartCoroutine(GameManager.httpClient.Accept(speakerStyle, _currentTrade, "none", AcceptCallback));
                    GoToNextTribe();
                    break;
                case false when algorithmDecisionEventArgs.counterOffer != null &&
                                algorithmDecisionEventArgs.counterOffer == _currentTrade:
                    //TEMPORARY decline when counter is same as original offer. 
                    StartCoroutine(GameManager.httpClient.Reject(speakerStyle, _currentTrade, reasonList.ToString(),
                        RejectCallback));
                    break;
                case false when algorithmDecisionEventArgs.counterOffer != null:
                    //Present counter offer to player
                    StartCoroutine(GameManager.httpClient.CounterOffer(speakerStyle, _currentTrade,
                        reasonList.ToString(), CounterOfferCallback));
                    ShowTradeOffer(algorithmDecisionEventArgs.counterOffer, _target, _originator);
                    break;
                default:
                    //Offer should be declined by the AI.
                    StartCoroutine(GameManager.httpClient.Reject(speakerStyle, _currentTrade, reasonList.ToString(),
                        RejectCallback));
                    break;
            }
        }

            
        private bool TradePossibleForTarget(Trade trade, Tribe target)
        {
            return target.Inventory.GetInventoryAmount(trade.RequestedItem) >= trade.RequestedAmount;
        }
            
        private bool TradePossibleForOriginator(Trade trade, Tribe originator)
        {
            return originator.Inventory.GetInventoryAmount(trade.OfferedItem) >= trade.OfferedAmount;
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