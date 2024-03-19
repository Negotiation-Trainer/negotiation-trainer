using TMPro;
using UnityEngine;

namespace Presenters
{
    public class SpeechPresenter : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField]private TMP_Text debugText;
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CheckBrowserSupported();
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SetupSpeechRecognition(string gameObjectName ,string liveTranscribeCallbackName, string finalResultCallbackName);
        
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StartSpeechRecognition();
    
        public void OnLiveSpeechTranscribe(string text)
        {
            if (debugMode)
            {
                debugText.text = text;
                Debug.Log(text);
            }
        }
        
        public void OnFinalSpeechRecognized(string result)
        {
            if (debugMode)
            {
                debugText.text = result;
                Debug.Log("final: " + result);
            }
        }

        public void StartRecognition()
        {
            StartSpeechRecognition();
        }
    
        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor && CheckBrowserSupported())
            {
                //setup javascript code to recognize speech.
                //given are the game object and functions on that object, that should be called when speech is recognized
                SetupSpeechRecognition("GameManager", "OnLiveSpeechTranscribe", "OnFinalSpeechRecognized");
            }
            else
            {
                Debug.LogWarning("Speech recognition not supported on your platform or browser");
            }
        }
    }
}
