using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class SettingsPresenter : MonoBehaviour
    {
        //pause menu
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private Slider voiceVolume;
        [SerializeField] private Toggle speechRecognitionToggle;
        public bool fallbackEnabled = false;
        
        //voice menu
        [SerializeField] private GameObject voiceSettings;
        [SerializeField] private TMP_Dropdown narratorVoice;
        [SerializeField] private Button narratorTest;
        [SerializeField] private TMP_Dropdown tribeBVoice;
        [SerializeField] private Button tribeBTest;
        [SerializeField] private TMP_Dropdown tribeCVoice;
        [SerializeField] private Button tribeCTest;
        [SerializeField] private Button okButton;

        private SpeechPresenter _speechPresenter;

        private void Start()
        {
            _speechPresenter = GetComponent<SpeechPresenter>();
            
            //settings menu
            voiceVolume.onValueChanged.AddListener(VoiceVolumeChanged);
            speechRecognitionToggle.onValueChanged.AddListener(FallbackEnabledChanged);
            
            //voice menu
            narratorVoice.onValueChanged.AddListener(NarratorVoiceSelected);
            tribeBVoice.onValueChanged.AddListener(TribeBVoiceSelected);
            tribeCVoice.onValueChanged.AddListener(TribeCVoiceSelected);
            narratorTest.onClick.AddListener(TestNarratorVoice);
            tribeBTest.onClick.AddListener(TestTribeBVoice);
            tribeCTest.onClick.AddListener(TestTribeCVoice);
            okButton.onClick.AddListener(OnOkButton);
        }
        
        public void ShowSettingsMenu(bool isActive)
        {
            voiceVolume.value = _speechPresenter.GetSpeechVolume();
            speechRecognitionToggle.isOn = fallbackEnabled;
            settingsMenu.SetActive(isActive);
        }
        private void VoiceVolumeChanged(float value)
        {
            _speechPresenter.SetSpeechVolume(value);
        }
        
        private void FallbackEnabledChanged(bool isActive)
        {
            fallbackEnabled = isActive;
        }
        
        private void SelectedVoiceChanged(int index)
        {
            _speechPresenter.SpeechVoice(index);
        }
        
        //voice menu
        public void ShowVoiceSettings(bool isActive)
        {
            narratorVoice.ClearOptions();
            tribeBVoice.ClearOptions();
            tribeCVoice.ClearOptions();
            
            narratorVoice.AddOptions(_speechPresenter.PossibleVoices().ToList());
            tribeBVoice.AddOptions(_speechPresenter.PossibleVoices().ToList());
            tribeCVoice.AddOptions(_speechPresenter.PossibleVoices().ToList());
            

            voiceSettings.SetActive(isActive);
        }

        private void NarratorVoiceSelected(int index)
        {
            _speechPresenter.selectedVoices[0] = index;
            _speechPresenter.StopSpeaking();
        }
        
        private void TribeBVoiceSelected(int index)
        {
            _speechPresenter.selectedVoices[1] = index;
            _speechPresenter.StopSpeaking();
        }
        
        private void TribeCVoiceSelected(int index)
        {
            _speechPresenter.selectedVoices[2] = index;
            _speechPresenter.StopSpeaking();
        }

        private void TestNarratorVoice()
        {
            _speechPresenter.Speak("Hello, This is the narrator speaking.","");
        }
        
        private void TestTribeBVoice()
        {
            _speechPresenter.Speak("Hello, This is tribe b speaking.",GameManager.Instance.Cpu1.Name);
        }
        
        private void TestTribeCVoice()
        {
            _speechPresenter.Speak("Hello, This is tribe c speaking.",GameManager.Instance.Cpu2.Name);
        }

        private void OnOkButton()
        {
            voiceSettings.SetActive(false);
            GameManager.Instance.ChangeGameState(GameManager.GameState.Start);
        }
    }
}
