using System;
using UnityEngine;

namespace SpeechServices
{
    public class WebGLSpeechToTextService : MonoBehaviour, ISpeechToTextService
    {
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CheckSTTBrowserSupported();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool IsListening();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupSpeechRecognition(string gameObjectName ,string liveTranscribeCallbackName, string finalResultCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StartSpeechRecognition();
        
        public bool CheckSupport()
        {
            if (CheckSTTBrowserSupported())
            {
                SetupSpeechRecognition(gameObject.name, nameof(OnLiveSpeechTranscribe), nameof(OnFinalSpeechRecognized));
                return true;
            }

            return false;
        }
        
        public void StartRecognition()
        {
            if (!IsListening())
            {
                StartSpeechRecognition();
            }
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
