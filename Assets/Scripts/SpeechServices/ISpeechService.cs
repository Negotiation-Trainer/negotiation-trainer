using System;

namespace SpeechServices
{
    public interface ISpeechService
    {
        public void StartRecognition();
        public bool CheckSupport(); 
        public event EventHandler<SpeechTranscribeEventArgs> SpeechTranscribe;
    }
    
    public class SpeechTranscribeEventArgs : EventArgs
    {
        public string Text;
        public bool IsFinal;
    }  
}