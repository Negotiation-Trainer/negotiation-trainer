using System;
using System.Collections;
using System.Collections.Generic;
using LogicServices;
using Presenters;
using UnityEngine;

public class DebugPresenter : MonoBehaviour
{
    private DialoguePresenter _dialoguePresenter;
    private SpeechPresenter _speechPresenter;

    private DialogueGenerationService _dialogueGenerationService;
    // Start is called before the first frame update
    void Start()
    {
        _dialoguePresenter = GetComponent<DialoguePresenter>();
        _speechPresenter = GetComponent<SpeechPresenter>();
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
                _speechPresenter.Speak("Hello this is ad debug text to test the speech to text capability of connor's paradise");
            }
        }
    }
}
