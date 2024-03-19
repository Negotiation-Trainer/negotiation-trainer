using TMPro;
using UnityEngine;

namespace Presenters
{
    public class SpeechPresenter : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField]private TMP_Text debugText;
    
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void StartSpeechRecognition(string gameObjectName ,string callbackName);
    
        public void OnSpeechRecognized(string result)
        {
            if (debugMode)
            {
                debugText.text = result;
                Debug.Log(result);
            }
        }
    
        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                //start javascript code to recognize speech.
                //given are the game object and function on that object, that should be called when speech is recognized
                StartSpeechRecognition("GameManager", "OnSpeechRecognized");
            }
            else
            {
                Debug.LogWarning("Speech recognition is only supported in WebGL builds.");
            }
        }
    }
}
