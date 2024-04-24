using System;

namespace SpeechServices
{
    public interface ITextToSpeechService
    {
        public event EventHandler FinishedSpeaking;
        public void SpeakText(string text);
        public void StopSpeech();
        public string[] GetPossibleVoices();
        public void SetSpeechVoice(int voice);
        public int GetSpeechVolume();
        public void SetSpeechVolume(int volume);
        public int GetSpeechRate();
        public void SetSpeechRate(int speakingRate);
        public bool CheckSupport();
    }
}