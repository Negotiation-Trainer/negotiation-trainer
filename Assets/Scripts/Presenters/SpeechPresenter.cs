using System;
using ModelLibrary;
using ServiceLibrary;
using SpeechServices;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class SpeechPresenter : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField] private GameObject error;
        [SerializeField] private TMP_Text errorMessage;
        private ISpeechToTextService _speechToTextService;
        private ITextToSpeechService _textToSpeechService;
        public bool speechToTextEnabled = false;
        public bool textToSpeechEnabled = false;
        public event EventHandler TTSFinished;

        private TradePresenter _tradePresenter;
        
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

            if (eventArgs.IsFinal)
            {
                try
                {
                    StartCoroutine(GameManager.httpClient.ConvertToTrade(eventArgs.Text, OnHTTPConvertCallback));
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

        private void OnHTTPConvertCallback(string response)
        {
            Trade trade = GameManager.httpClient.ConvertToTrade(response);
            
            Debug.Log($"trade: {trade}");
            _tradePresenter.ShowTradeOffer(trade, GameManager.Instance.Player, GameManager.Instance.Cpu1);
        }

        private void OnTextToSpeechFinished(object sender, EventArgs eventArgs)
        {
            TTSFinished?.Invoke(this,EventArgs.Empty);
        }

        private void ShowErrors()
        {
            if (!textToSpeechEnabled && !speechToTextEnabled)
            {
                errorMessage.text = "Speech recogonition and synthesis unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari";
            } else if (!speechToTextEnabled)
            {
                errorMessage.text = "Speech recogonition unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari";
            }
            else
            {
                errorMessage.text = "Speech synthesis unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari";
            }
            error.SetActive(true);
        }

        public void HideError()
        {
            error.SetActive(false);
        }
    
        void Start()
        {
            _tradePresenter = GetComponent<TradePresenter>();
            
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
