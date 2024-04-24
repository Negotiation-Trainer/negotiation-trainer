using System;
using System.Linq;
using UnityEngine;

namespace SpeechServices
{
    public class WebGLTextToSpeechService : MonoBehaviour, ITextToSpeechService
    {
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CheckTTSBrowserSupported();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupTextToSpeech(int volume, int rate, int pitch, string gameObjectName, string onEndCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Speak(string text);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StopSpeaking();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string GetVoices();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetVoice(int voiceIndex);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int GetVolume();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetVolume(int volume);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int GetSpeakingRate();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetSpeakingRate(int speakingRate);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Pause();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Resume();
        public event EventHandler FinishedSpeaking;


        public string[] GetPossibleVoices()
        {
            string[] voices = GetVoices().Split('#');
            voices = voices.Select(voice => voice.Length > 27 ? voice.Substring(0, 27) : voice).ToArray();
            return voices;
        }
        
        public void SetSpeechVoice(int voice)
        {
            SetVoice(voice);
        }
        
        public int GetSpeechVolume()
        {
            return GetVolume();
        }
        
        public void SetSpeechVolume(int volume)
        {
            SetVolume(volume);
        }
        
        public int GetSpeechRate()
        {
            return GetSpeakingRate();
        }
        
        public void SetSpeechRate(int speakingRate)
        {
            SetSpeakingRate(speakingRate);
        }
        
        public bool CheckSupport()
        {
            if (CheckTTSBrowserSupported())
            {
                SetupTextToSpeech(1,1,1, gameObject.name, nameof(OnSpeechEnded));
                return true;
            }

            return false;
        }
        
        private void Start()
        {
            SetupTextToSpeech(1,1,1, gameObject.name, nameof(OnSpeechEnded) );
        }
        
        public void SpeakText(string text)
        {
            if(text == null) return;
            Speak(text);
        }

        public void StopSpeech()
        {
            StopSpeaking();
        }
        
        public void OnSpeechEnded()
        {
            FinishedSpeaking?.Invoke(this,EventArgs.Empty);
        }
    }
}
