using System;
using LogicServices;
using SpeechServices;
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
    
        void Start()
        {
            var dialoguePresenter = GetComponent<DialoguePresenter>();
            var dialogueGenerationService = new DialogueGenerationService();
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                _speechToTextService = gameObject.AddComponent<WebGLSpeechToTextService>();
                if (!_speechToTextService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLSpeechToTextService>());
                    Debug.LogWarning("Speech recognition not supported on your platform or browser");
                    dialoguePresenter.QueueMessages(dialogueGenerationService.SplitTextToInstructionMessages("Speech recognition not supported on your platform or browser"));
                    dialoguePresenter.ShowNextMessage();
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
                    Debug.LogWarning("Speech synthesis not supported on your platform or browser");
                    dialoguePresenter.QueueMessages(dialogueGenerationService.SplitTextToInstructionMessages("Speech synthesis not supported on your platform or browser"));
                    dialoguePresenter.ShowNextMessage();
                }
                else
                {
                    textToSpeechEnabled = true;
                    _textToSpeechService.FinishedSpeaking += OnTextToSpeechFinished; 
                }
            }
        }
    }
}
