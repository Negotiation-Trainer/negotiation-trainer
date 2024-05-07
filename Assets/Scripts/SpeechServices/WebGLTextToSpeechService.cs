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
        private static extern float GetVolume();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetVolume(float volume);
        
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
            voices = voices.Select(voice => voice.Length > 26 ? voice.Substring(0, 26).TrimEnd('(') : voice).ToArray();
            return voices;
        }
        
        public void SetSpeechVoice(int voice)
        {
            SetVoice(voice);
        }
        
        public float GetSpeechVolume()
        {
            return GetVolume();
        }
        
        public void SetSpeechVolume(float volume)
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

        public void PauseSpeech()
        {
            Pause();
        }

        public void ResumeSpeech()
        {
            Resume();
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
