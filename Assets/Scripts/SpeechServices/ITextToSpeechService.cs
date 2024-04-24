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
        public float GetSpeechVolume();
        public void SetSpeechVolume(float volume);
        public int GetSpeechRate();
        public void SetSpeechRate(int speakingRate);
        public void PauseSpeech();
        public void ResumeSpeech(); 
        public bool CheckSupport();
    }
}