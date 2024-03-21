using System;
using System.Collections.Generic;
using LogicServices;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class DialoguePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private Button nextDialogueButton;
        //[SerializeField] private Button DebugDialogueButton;

        private Queue<IMessage> _dialogueQue = new Queue<IMessage>();
        private DialogueGenerationService _dialogueGenerationService = new DialogueGenerationService();

        public void QueMessages(IMessage[] messages)
        {
            foreach (var message in messages)
            {
                _dialogueQue.Enqueue(message);
            }
        }
        
        public void ShowNextMessage()
        {
            if (_dialogueQue.Count == 0)
            {
                dialogueBox.SetActive(false);
                return;
            }
            if(!dialogueBox.activeSelf) dialogueBox.SetActive(true);
            IMessage message = _dialogueQue.Dequeue();
            dialogueText.text = message.Message;
        }

        public void StopDialogue()
        {
            _dialogueQue = new Queue<IMessage>();
            dialogueBox.SetActive(false);
        }
        
        private void DebugStartDialogue()
        {
            DialogueGenerationService dgs = new DialogueGenerationService();
            
            var testMessages = dgs.SplitTextToDialogueMessages(
                "hello this is the first message {nm} this is second {nm} and this is the third message", 1);
            
            IMessage[] messages = {
                new InstructionMessage("Hello this is the first message of the dialogue service"),
                new InstructionMessage("This is the second message"),
                new InstructionMessage("The third message is the last of the que")
            };
            
            QueMessages(testMessages);
            ShowNextMessage();
        }

        private void Start()
        {
            nextDialogueButton.onClick.AddListener(ShowNextMessage);
            //DebugDialogueButton.onClick.AddListener(DebugStartDialogue);
        }

        public void StartGeneralInstruction()
        {
            QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("general")));
            ShowNextMessage();
        }
        
        /*//Debug UI
        void OnGUI()
        {
            if (GUILayout.Button("Tribe A"))
            {
                QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("a")));
                ShowNextMessage();
            }
            if (GUILayout.Button("Tribe B"))
            {
                QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("b")));
                ShowNextMessage();
            }
            if (GUILayout.Button("Tribe C"))
            {
                QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("c")));
                ShowNextMessage();
            }
            if (GUILayout.Button("General"))
            {
                QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("general")));
                ShowNextMessage();
            }
        }*/

        #region Temporary instruction service

        public string GetInstruction(string tribe)
        {
            switch (tribe)
            {
                case "a":
                    return GetInstructionTextFromFile("TribeA");
                case "b":
                    return GetInstructionTextFromFile("TribeB");
                case "c":
                    return GetInstructionTextFromFile("TribeC");
                default:
                    return GetInstructionTextFromFile("general");
            }
        }
        private string GetInstructionTextFromFile(string path)
        {
            string textFile = Resources.Load(path).ToString();
            return textFile;
        }
        
        #endregion
    }
}
