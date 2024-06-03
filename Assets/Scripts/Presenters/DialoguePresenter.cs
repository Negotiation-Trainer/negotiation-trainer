using System;
using System.Collections.Generic;
using ModelLibrary;
using ModelLibrary.Interfaces;
using ServiceLibrary;
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
        [SerializeField] private GameObject nameBox;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button nextDialogueButton;
        private SpeechPresenter _speechPresenter;
        private DialogueGenerationService _dialogueGenerationService = new DialogueGenerationService();
        private Queue<IMessage> _dialogueQueue = new Queue<IMessage>();
        public event EventHandler DialogueFinished;

        public void QueueMessages(IMessage[] messages)
        {
            foreach (var message in messages)
            {
                _dialogueQueue.Enqueue(message);
            }
        }

        public int MessagesRemaining()
        {
            return _dialogueQueue.Count;
        }
        
        public void ShowNextMessage()
        {
            Debug.Log(_dialogueQueue.Count);
            
            string tribeName = "";
            
            if (_dialogueQueue.Count == 0)
            {
                dialogueBox.SetActive(false);
                DialogueFinished?.Invoke(this, EventArgs.Empty);
                return;
            }
            if(!dialogueBox.activeSelf) dialogueBox.SetActive(true);
            
            IMessage message = _dialogueQueue.Dequeue();
            
            if (message.GetType() == typeof(DialogueMessage))
            {
                ShowCharacter((DialogueMessage)message);
                DialogueMessage msg = (DialogueMessage) message;
                tribeName = msg.TribeName;
            }
            else
            {
                portraits.SetActive(false);
            }
            
            dialogueText.text = message.Message;
            if (_speechPresenter)
            {
                _speechPresenter.Speak(message.Message, tribeName);
            }
        }

        private void ShowCharacter(DialogueMessage message)
        {
            portraits.SetActive(true);
            nameBox.SetActive(true);
            nameText.text = message.TribeName;
        }

        public void StopDialogue()
        {
            _dialogueQueue = new Queue<IMessage>();
            dialogueBox.SetActive(false);
        }
        
        private void OnTTSFinished(object sender, EventArgs eventArgs)
        {
            ShowNextMessage();
        }

        private void OnNextButtonClick()
        {
            if (_speechPresenter)
            {
                _speechPresenter.StopSpeaking();
            }

            ShowNextMessage();
        }

        private void Start()
        {
            nextDialogueButton.onClick.AddListener(OnNextButtonClick);
            _speechPresenter = GetComponent<SpeechPresenter>();
            if (_speechPresenter)
            {
                _speechPresenter.TTSFinished += OnTTSFinished;
            }
        }

        public void EnqueueAndShowDialogueString(string message, string tribeName)
        {
            QueueMessages(_dialogueGenerationService.SplitTextToDialogueMessages(message, tribeName));
            ShowNextMessage();
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
                case "Intermission":
                    return GetInstructionTextFromFile("Intermission");
                case "Ending":
                    return GetInstructionTextFromFile("Ending");
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
