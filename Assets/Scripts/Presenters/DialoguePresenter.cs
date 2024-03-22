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

        private Queue<IMessage> _dialogueQue = new Queue<IMessage>();
        private DialogueGenerationService _dialogueGenerationService = new();

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
            _dialogueQue = new Queue<IMessage>();
            dialogueBox.SetActive(false);
        }
        
        private void Start()
        {
            nextDialogueButton.onClick.AddListener(ShowNextMessage);
        }

        public void StartGeneralInstruction()
        {
            QueMessages(_dialogueGenerationService.SplitTextToInstructionMessages(GetInstruction("general")));
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
