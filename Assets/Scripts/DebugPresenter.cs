using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using LogicServices;
using Models;
using Presenters;
using UnityEngine;

public class DebugPresenter : MonoBehaviour
{
    private GameManager _gameManager;
    private DialoguePresenter _dialoguePresenter;
    private SpeechPresenter _speechPresenter;
    private TradePresenter _tradePresenter;

    [SerializeField] private InventoryItems debugRequestedItem = InventoryItems.Wood;
    [SerializeField] private int debugRequestedAmount = 2;
    [SerializeField] private InventoryItems debugOfferedItem = InventoryItems.Steel;
    [SerializeField] private int debugOfferedAmount = 2;

    private DialogueGenerationService _dialogueGenerationService;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _dialoguePresenter = GetComponent<DialoguePresenter>();
        _speechPresenter = GetComponent<SpeechPresenter>();
        _tradePresenter = GetComponent<TradePresenter>();

        StartServices();
    }

    void StartServices()
    {
        _dialogueGenerationService = new DialogueGenerationService();
    }

    void OnGUI()
    {
        if (_dialoguePresenter)
        {
            GUILayout.Label("Dialogue");
            if (GUILayout.Button("Tribe A"))
            {
                _dialoguePresenter.QueueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("a")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Tribe B"))
            {
                _dialoguePresenter.QueueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("b")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Tribe C"))
            {
                _dialoguePresenter.QueueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("c")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("General"))
            {
                _dialoguePresenter.QueueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(
                        _dialoguePresenter.GetInstruction("general")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Dialogue"))
            {
                _dialoguePresenter.QueueMessages(
                    _dialogueGenerationService.SplitTextToDialogueMessages(_dialoguePresenter.GetInstruction("general"),
                        1));
                _dialoguePresenter.ShowNextMessage();
            }
        }

        if (_speechPresenter)
        {
            GUILayout.Label("Speech");
            if (GUILayout.Button("Speak"))
            {
                _speechPresenter.Speak(
                    "Hello this is a debug text to test the speech to text capability of connor's paradise");
            }

            if (GUILayout.Button("Stop speak"))
            {
                _speechPresenter.StopSpeaking();
                if (_tradePresenter)
                {
                    GUILayout.Label("Trade");
                    if (GUILayout.Button("Show trade offer to player"))
                    {
                        var trade = new Trade(debugRequestedItem, debugRequestedAmount, debugOfferedItem,
                            debugOfferedAmount);
                        _tradePresenter.ShowTradeOffer(trade, _gameManager.Cpu1, _gameManager.Player);
                    }

                    if (GUILayout.Button("Show trade offer from player"))
                    {
                        var trade = new Trade(debugRequestedItem, debugRequestedAmount, debugOfferedItem,
                            debugOfferedAmount);
                        _tradePresenter.ShowTradeOffer(trade, _gameManager.Player, _gameManager.Cpu1);
                    }
                }
            }
        }
    }
}
