using System;

namespace SpeechServices
{
    public interface ISpeechToTextService
    {
        public event EventHandler<SpeechTranscribeEventArgs> SpeechTranscribe;
        public void StartRecognition();
        public bool CheckSupport(); 
    }
    
    public class SpeechTranscribeEventArgs : EventArgs
    {
        public string Text;
        public bool IsFinal;
    }  
}