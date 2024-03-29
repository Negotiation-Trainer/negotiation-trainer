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
        [SerializeField] private GameObject portraits;
        [SerializeField] private Button nextDialogueButton;
        
        private DialogueGenerationService _dialogueGenerationService = new DialogueGenerationService();
        private Queue<IMessage> _dialogueQueue = new Queue<IMessage>();

        public void QueueMessages(IMessage[] messages)
        {
            foreach (var message in messages)
            {
                _dialogueQueue.Enqueue(message);
            }
        }
        
        public void ShowNextMessage()
        {
            if (_dialogueQueue.Count == 0)
            {
                dialogueBox.SetActive(false);
                return;
            }
            if(!dialogueBox.activeSelf) dialogueBox.SetActive(true);
            
            IMessage message = _dialogueQueue.Dequeue();
            
            if (message.GetType() == typeof(DialogueMessage))
            {
                ShowCharacter((DialogueMessage)message);
            }
            else
            {
                portraits.SetActive(false);
            }

            dialogueText.text = message.Message;
        }

        private void ShowCharacter(DialogueMessage message)
        {
            switch (message.TribeId)
            {
                case 0:
                    portraits.SetActive(true);
                    //Tribe a portrait
                    break;
                case 1 :
                    portraits.SetActive(true);
                    //Tribe b portrait
                    break;
                case 3 :
                    portraits.SetActive(true);
                    //Tribe b portrait
                    break;
            }
        }

        public void StopDialogue()
        {
            _dialogueQueue = new Queue<IMessage>();
            dialogueBox.SetActive(false);
        }
        
        private void DebugStartDialogue()
        {
            IMessage[] messages = {
                new InstructionMessage("Hello this is the first message of the dialogue service"),
                new InstructionMessage("This is the second message"),
                new InstructionMessage("The third message is the last of the que")
            };
            QueueMessages(messages);
            ShowNextMessage();
        }

        private void Start()
        {
            nextDialogueButton.onClick.AddListener(ShowNextMessage);
        }

        public void StartGeneralInstruction()
        {
            QueueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("general")));
            ShowNextMessage();
        }

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
