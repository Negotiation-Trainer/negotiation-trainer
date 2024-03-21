using System;
using System.Collections.Generic;
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
        [SerializeField] private Button DebugDialogueButton;

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
            dialogueText.text = message.Message;
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
            DebugDialogueButton.onClick.AddListener(DebugStartDialogue);
        }
    }
}
