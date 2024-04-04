using System;
using SpeechServices;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class SpeechPresenter : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        private ISpeechToTextService _speechToTextService;
        private ITextToSpeechService _textToSpeechService;
        public event EventHandler TTSFinished;
        
        public void StartRecognition()
        {
            _speechToTextService?.StartRecognition();
        }

        public void Speak(string text)
        {
            _textToSpeechService?.SpeakText(text);
        }

        public void StopSpeaking()
        {
            _textToSpeechService?.StopSpeech();
        }

        private void OnSpeechToTextTranscribe(object sender, SpeechTranscribeEventArgs eventArgs)
        {
            if (debugMode)
            {
                Debug.Log($"{eventArgs.Text} -isFinal:{eventArgs.IsFinal}");
            }
        }

        private void OnTextToSpeechFinished(object sender, EventArgs eventArgs)
        {
            TTSFinished?.Invoke(this,EventArgs.Empty);
        }
    
        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                _speechToTextService = gameObject.AddComponent<WebGLSpeechToTextService>();
                if (!_speechToTextService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLSpeechToTextService>());
                    Debug.LogWarning("Speech recognition not supported on your platform or browser");
                }
                else
                {
                    _speechToTextService.SpeechTranscribe += OnSpeechToTextTranscribe;   
                }

                _textToSpeechService = gameObject.AddComponent<WebGLTextToSpeechService>();
                if (!_speechToTextService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLTextToSpeechService>());
                    Debug.LogWarning("Speech synthesis not supported on your platform or browser");
                }
                else
                {
                    _textToSpeechService.FinishedSpeaking += OnTextToSpeechFinished; 
                }
            }
        }
    }
}
