using System;
using Cinemachine;
using ServiceLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class CutscenePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject island;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private GameObject board;
        [SerializeField] private float speed = 1f;
        private bool _transitioning = false;

        [SerializeField] private ScrollAndFadeTexture fogScript;
        [SerializeField] private FanController fanScript;

        [SerializeField] private GameObject rainbow;
        [SerializeField] private GameObject stormIncoming;
        [SerializeField] private GameObject storm;
        [SerializeField] private GameObject softClouds;
        
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        [SerializeField] private int[] switchMoment;
        [SerializeField] private bool switchCamera = false;
        [SerializeField] private Light light;
        private float _t = 0;
        private bool _darken = false;
        private float _max;
        private float _min;
        private DialoguePresenter _dialoguePresenter;
        private readonly DialogueGenerationService _dialogueGenerationService = new DialogueGenerationService();
        

        private int index = 0;
        // Start is called before the first frame update
        private void Start()
        {
            _dialoguePresenter = GetComponent<DialoguePresenter>();
        }

        public void Darken()
        {
            _min = 0;
            _max = 2.5f;
            _t = 0;
            _darken = true;
        }

        public void UnDarken()
        {
            _min = 2.5f;
            _max = 0;
            _t = 0;
            _darken = true;
        }

        public void StartMoveIsland()
        {
            Invoke(nameof(MoveIsland), 1f);
        }

        public void StartGame()
        {
            switchCamera = true;
        }

        private void MoveIsland()
        {
            _transitioning = true;
        }

        public void ToggleRainbow(bool isActive)
        {
            rainbow.SetActive(isActive);
        }

        public void ToggleStormIncoming(bool stormIncomingActive)
        {
            if (stormIncomingActive)
            {
                softClouds.GetComponent<ParticleSystem>().Stop();
                stormIncoming.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                stormIncoming.GetComponent<ParticleSystem>().Stop();
            }
            
        }

        private void FixedUpdate()
        {
            if (_transitioning)
            {
                island.transform.position = Vector3.MoveTowards(island.transform.position, targetPosition, speed);
                if (island.transform.position.Compare(targetPosition, 100))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToInstructionMessages(
                            _dialoguePresenter.GetInstruction("general")));
                    _dialoguePresenter.ShowNextMessage();
                    UnDarken();
                    fogScript.StopFade();
                    fanScript.isActive = true;
                    board.SetActive(false);
                    _transitioning = false;
                }
            }
        }
        
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
