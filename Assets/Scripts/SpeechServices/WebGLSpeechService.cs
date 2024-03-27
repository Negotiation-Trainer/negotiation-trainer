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
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupTextToSpeech(int voice, int volume, int rate, int pitch, string gameObjectName, string onEndCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Speak(string text);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StopSpeaking();

        private void Start()
        {
            //setup javascript code to recognize speech.
            //given are the game object and functions on that object, that should be called when speech is recognized
            SetupSpeechRecognition(gameObject.name, nameof(OnLiveSpeechTranscribe), nameof(OnFinalSpeechRecognized));
            SetupTextToSpeech(1,1,1,1, gameObject.name, nameof(OnSpeechEnded) );
        }

        public void StartRecognition()
        {
            if (!IsListening())
            {
                StartSpeechRecognition();
            }
        }

        public void SpeakText(string text)
        {
            Speak(text);
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

        public void OnSpeechEnded()
        {
            //tts ended
            Debug.Log("The TTS has ended");
        }
    }
}
