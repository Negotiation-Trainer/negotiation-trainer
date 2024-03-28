using System;
using UnityEngine;

namespace SpeechServices
{
    public class WebGLSpeechService : MonoBehaviour, ISpeechService
    {
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CheckBrowserSupported();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsListening();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupSpeechRecognition(string gameObjectName ,string liveTranscribeCallbackName, string finalResultCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StartSpeechRecognition();

        private void Start()
        {
            //setup javascript code to recognize speech.
            //given are the game object and functions on that object, that should be called when speech is recognized
            SetupSpeechRecognition("GameManager", "OnLiveSpeechTranscribe", "OnFinalSpeechRecognized");
        }

        public void StartRecognition()
        {
            if (!IsListening())
            {
                StartSpeechRecognition();
            }
        }

        public bool CheckSupport()
        {
            return CheckBrowserSupported();
        }

        public event EventHandler<SpeechTranscribeEventArgs> SpeechTranscribe;
        
        public void OnLiveSpeechTranscribe(string text)
        {
            SpeechTranscribe?.Invoke(this, new SpeechTranscribeEventArgs{IsFinal = false, Text = text});
        }
        
        public void OnFinalSpeechRecognized(string text)
        {
            SpeechTranscribe?.Invoke(this, new SpeechTranscribeEventArgs{IsFinal = true, Text = text});
        }
    }
}
