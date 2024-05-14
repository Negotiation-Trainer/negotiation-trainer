using TMPro;
using UnityEngine;

namespace Presenters
{
    public class ErrorPresenter : MonoBehaviour
    {
        private static ErrorPresenter Instance { get; set; }
        [SerializeField] private GameObject errorBox;
        [SerializeField] private TMP_Text errorText;
        void Awake()
        {
            Instance = this;
        }

        public static void ShowError(string errorMessage)
        {
            if (Instance == null) return;
            Instance.errorText.text = errorMessage;
            Instance.errorBox.SetActive(true);
        }
        
        public static void HideError()
        {
            if (Instance == null) return;
            Instance.errorBox.SetActive(false);
        }

        
    }
}
