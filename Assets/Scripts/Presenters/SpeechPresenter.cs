using SpeechServices;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class SpeechPresenter : MonoBehaviour
    {
        [SerializeField] private bool debugMode = false;
        [SerializeField]private TMP_Text debugText;
        private ISpeechService _speechService;
        
        
        
        public void StartRecognition()
        {
            _speechService.StartRecognition();
        }

        private void OnSpeechTranscribe(object sender, SpeechTranscribeEventArgs eventArgs)
        {
            if (debugMode)
            {
                debugText.text = eventArgs.Text;
                Debug.Log(eventArgs.IsFinal);
            }
        }
    
        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer && !Application.isEditor)
            {
                _speechService = gameObject.AddComponent<WebGLSpeechService>();
                if (!_speechService.CheckSupport())
                {
                    Destroy(GetComponent<WebGLSpeechService>());
                    return;
                }
                _speechService.SpeechTranscribe += OnSpeechTranscribe;
            }
            Debug.LogWarning("Speech recognition not supported on your platform or browser");
        }
    }
}
