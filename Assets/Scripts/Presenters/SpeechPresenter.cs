using System;
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
        
        /// <summary>
        /// Start listening to users mic.
        /// fires TTSFinished with the result. 
        /// </summary>
        public void StartRecognition()
        {
            if (!speechToTextEnabled) return;
            _speechToTextService?.StartRecognition();
        }

        /// <summary>
        /// Have TTS speak a text.
        /// </summary>
        /// <param name="text">Text to speak</param>
        public void Speak(string text)
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService?.SpeakText(text);
        }

        /// <summary>
        /// Force TTS to stop talking.
        /// </summary>
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
        
        /// <summary>
        /// Get all available voices on the machine.
        /// </summary>
        /// <returns>String array with voice names</returns>
        public string[] PossibleVoices()
        {
            if (!textToSpeechEnabled) return new []{""};
            return _textToSpeechService.GetPossibleVoices();
        }
        
        /// <summary>
        /// Set the TTS voice.
        /// </summary>
        /// <param name="voice">index of voice in the PossibleVoices array</param>
        public void SpeechVoice(int voice)
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService.SetSpeechVoice(voice);
        }

        /// <summary>
        /// Get current TTS volume.
        /// </summary>
        /// <returns>Float between 0 and 1</returns>
        public float GetSpeechVolume()
        {
            if (!textToSpeechEnabled) return 0;
            return _textToSpeechService.GetSpeechVolume();
        }

        /// <summary>
        /// Set TTS volume.
        /// </summary>
        /// <param name="volume">Float between 0 and 1</param>
        public void SetSpeechVolume(float volume)
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService.SetSpeechVolume(volume);
        }

        /// <summary>
        /// Get current speaking rate of the TTS.
        /// </summary>
        /// <returns>Int between 1 and 10</returns>
        public int GetSpeechRate()
        {
            if (!textToSpeechEnabled) return 0;
            return _textToSpeechService.GetSpeechRate();
        }

        /// <summary>
        /// Set speaking rate of the tts.
        /// </summary>
        /// <param name="speakingRate">Int between 1 and 10</param>
        public void SetSpeechRate(int speakingRate)
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService.SetSpeechRate(speakingRate);
        }

        /// <summary>
        /// Pause TTS. Will not speak again until Resume is called
        /// </summary>
        public void Pause()
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService.PauseSpeech();
        }
        
        /// <summary>
        /// Resume TTS if it was previously paused.
        /// </summary>
        public void Resume()
        {
            if (!textToSpeechEnabled) return;
            _textToSpeechService.ResumeSpeech();
        }
        
        private void ShowErrors()
        {
            if (!textToSpeechEnabled && !speechToTextEnabled)
            {
                errorMessage.text = "Speech recognition and synthesis unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari";
            } else if (!speechToTextEnabled)
            {
                errorMessage.text = "Speech recognition unsupported in current browser. \nplease use one of the following:\nChrome, Edge, Opera or Safari";
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
