using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechPresenter : MonoBehaviour
{
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void StartSpeechRecognition(string gameObjectName ,string callbackName);
    
    public void OnSpeechRecognized(string result)
    {
        Debug.Log(result);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Call JavaScript function to start speech recognition
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartSpeechRecognition("GameManager", "OnSpeechRecognized");
        }
        else
        {
            Debug.LogWarning("Speech recognition is only supported in WebGL builds.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
