using System.Collections.Generic;
using Models;

namespace LogicServices
{
    public class DialogueGenerationService
    {
        public DialogueMessage[] SplitTextToMessages(string text, int tribeId)
        {
            string[] messages = text.Split("{nm}");
            DialogueMessage[] dialogueMessages = new DialogueMessage[messages.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                dialogueMessages[i] = new DialogueMessage(tribeId, messages[i]);
            }

            return dialogueMessages;
        }
    }
}