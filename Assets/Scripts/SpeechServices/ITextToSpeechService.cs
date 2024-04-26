using System;

namespace SpeechServices
{
    public interface ITextToSpeechService
    {
        public event EventHandler FinishedSpeaking;
        public void SpeakText(string text);
        public void StopSpeech();
        public bool CheckSupport(); 
    }
}