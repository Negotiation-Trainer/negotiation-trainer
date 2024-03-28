using System;
using UnityEngine;

namespace SpeechServices
{
    public class WebGLTextToSpeechService : MonoBehaviour, ITextToSpeechService
    {
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CheckTTSBrowserSupported();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupTextToSpeech(int voice, int volume, int rate, int pitch, string gameObjectName, string onEndCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Speak(string text);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StopSpeaking();
        
        public event EventHandler FinishedSpeaking;
        
        public bool CheckSupport()
        {
            return CheckTTSBrowserSupported();
        }

        private void Start()
        {
            SetupTextToSpeech(1,1,1,1, gameObject.name, nameof(OnSpeechEnded) );
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
