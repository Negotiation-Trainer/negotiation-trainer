using Cinemachine;
using UnityEngine;

namespace Presenters
{
    public class CutscenePresenter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        [SerializeField] private int[] switchMoment;
        [SerializeField] private bool switchCamera = false;
        [SerializeField] private Light light;
        private float _t = 0;
        private bool _darken = false;
        private float _max;
        private float _min;
        private DialoguePresenter _dialoguePresenter;

        private int index = 0;
        // Start is called before the first frame update
        private void Start()
        {
            _dialoguePresenter = GetComponent<DialoguePresenter>();
        }

        public void Darken()
        {
            _min = 0;
            _max = 1;
            _t = 0;
            _darken = true;
        }

        public void UnDarken()
        {
            _min = 1;
            _max = 0;
            _t = 0;
            _darken = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_darken)
            {
                light.intensity = Mathf.Lerp(_max, _min, _t);
                _t += 0.5f * Time.deltaTime;
                if (light.intensity == _min) _darken = false;
            }

            if (switchMoment.Length -1 != index)
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
}
