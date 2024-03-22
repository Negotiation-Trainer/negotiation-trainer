using System;
using System.Collections;
using System.Collections.Generic;
using LogicServices;
using Presenters;
using UnityEngine;

public class DebugPresenter : MonoBehaviour
{
    private DialoguePresenter _dialoguePresenter;

    private DialogueGenerationService _dialogueGenerationService;
    // Start is called before the first frame update
    void Start()
    {
        _dialoguePresenter = GetComponent<DialoguePresenter>();
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
                _dialoguePresenter.QueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("a")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Tribe B"))
            {
                _dialoguePresenter.QueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("b")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Tribe C"))
            {
                _dialoguePresenter.QueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(_dialoguePresenter.GetInstruction("c")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("General"))
            {
                _dialoguePresenter.QueMessages(
                    _dialogueGenerationService.SplitTextToInstructionMessages(
                        _dialoguePresenter.GetInstruction("general")));
                _dialoguePresenter.ShowNextMessage();
            }

            if (GUILayout.Button("Dialogue"))
            {
                _dialoguePresenter.QueMessages(
                    _dialogueGenerationService.SplitTextToDialogueMessages(_dialoguePresenter.GetInstruction("general"),
                        1));
                _dialoguePresenter.ShowNextMessage();
            }
        }
    }
}
