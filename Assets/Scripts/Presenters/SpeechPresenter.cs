using System;
using ServiceLibrary;
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
        public bool speechToTextEnabled = false;
        public bool textToSpeechEnabled = false;
        public event EventHandler TTSFinished;
        
        public void StartRecognition()
        {
            if (!speechToTextEnabled) return;
            _speechToTextService?.StartRecognition();
        }

        public void Speak(string text)
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService?.SpeakText(text);
        }

        public void StopSpeaking()
        {
            if (!textToSpeechEnabled) return;
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

        private void ShowErrors()
        {
            if (!textToSpeechEnabled && !speechToTextEnabled)
            {
                ErrorPresenter.ShowError("Speech recogonition and synthesis unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari");
            } else if (!speechToTextEnabled)
            {
                ErrorPresenter.ShowError("Speech recogonition unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari");
            }
            else
            {
                ErrorPresenter.ShowError("Speech synthesis unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari");
            }
        }

        public void HideError()
        {
            ErrorPresenter.HideError();
        }
    
        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                _speechToTextService = gameObject.AddComponent<WebGLSpeechToTextService>();
                if (!_speechToTextService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLSpeechToTextService>());
                }
                else
                {
                    speechToTextEnabled = true;
                    _speechToTextService.SpeechTranscribe += OnSpeechToTextTranscribe;
                }

                _textToSpeechService = gameObject.AddComponent<WebGLTextToSpeechService>();
                if (!_textToSpeechService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLTextToSpeechService>());
                }
                else
                {
                    textToSpeechEnabled = true;
                    _textToSpeechService.FinishedSpeaking += OnTextToSpeechFinished; 
                }
            }

            if (!textToSpeechEnabled || !speechToTextEnabled)
            {
                ShowErrors();
            }
        }
    }
}
