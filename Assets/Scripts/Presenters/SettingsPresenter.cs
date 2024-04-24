using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class SettingsPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private TMP_Dropdown selectedVoice;
        [SerializeField] private Slider voiceVolume;
        private SpeechPresenter _speechPresenter;

        private void Start()
        {
            _speechPresenter = GetComponent<SpeechPresenter>();
            selectedVoice.onValueChanged.AddListener(SelectedVoiceChanged);
            voiceVolume.onValueChanged.AddListener(VoiceVolumeChanged);
        }

        public void ShowSettingsMenu(bool isActive)
        {
            selectedVoice.ClearOptions();
            selectedVoice.AddOptions(_speechPresenter.PossibleVoices().ToList());
            voiceVolume.value = _speechPresenter.GetSpeechVolume();
            settingsMenu.SetActive(isActive);
        }

        private void SelectedVoiceChanged(int index)
        {
            _speechPresenter.SpeechVoice(index);
        }

        private void VoiceVolumeChanged(float value)
        {
            _speechPresenter.SetSpeechVolume(value);
        }
    }
}
