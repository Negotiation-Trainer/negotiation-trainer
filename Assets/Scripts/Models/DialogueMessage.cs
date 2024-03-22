namespace Models
{
    public class DialogueMessage : IMessage
    {
        public int TribeId { get; private set; }
        public string Message { get; private set; }

        public DialogueMessage(int tribeId, string message)
        {
            TribeId = tribeId;
            Message = message;
        }
    }
}