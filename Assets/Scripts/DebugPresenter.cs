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
    private DialoguePresenter _dialoguePresenter;
    private TradePresenter _tradePresenter;
    
    private DialogueGenerationService _dialogueGenerationService;
    // Start is called before the first frame update
    void Start()
    {
        _dialoguePresenter = GetComponent<DialoguePresenter>();
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

        if (_tradePresenter)
        {
            GUILayout.Label("Trade");
            if (GUILayout.Button("Show trade offer to player"))
            {
                var trade = new Trade(InventoryItems.Wood, 2, InventoryItems.Stone, 2);
                var tribeA = new Tribe("A");
                var tribeB = new Tribe("B");
                
                _tradePresenter.ShowTradeOffer(trade,tribeB,tribeA);
            }
            
            if (GUILayout.Button("Show trade offer from player"))
            {
                var trade = new Trade(InventoryItems.Wood, 2, InventoryItems.Stone, 2);
                var tribeA = new Tribe("A");
                var tribeB = new Tribe("B");
                
                _tradePresenter.ShowTradeOffer(trade,tribeA,tribeB);
            }
        }
    }
}
