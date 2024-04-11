using Cinemachine;
using UnityEngine;

namespace Presenters
{
    public class CutscenePresenter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        [SerializeField] private int[] switchMoment;
        [SerializeField] private bool switchCamera = false;
        private DialoguePresenter _dialoguePresenter;

        private int index = 0;
        // Start is called before the first frame update
        private void Start()
        {
            _dialoguePresenter = GetComponent<DialoguePresenter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (switchMoment[index] == _dialoguePresenter.MessagesRemaining() || switchCamera)
            {
                virtualCameras[index].gameObject.SetActive(false);
                index++;
                virtualCameras[index].gameObject.SetActive(true);
                switchCamera = false;
            }
        }
    }
}
