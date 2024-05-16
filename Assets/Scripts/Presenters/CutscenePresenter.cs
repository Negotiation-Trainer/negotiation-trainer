using System;
using Cinemachine;
using ServiceLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class CutscenePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject animals;
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
        [SerializeField] private CinemachineVirtualCamera[] intermissionVirtualCameras;
        [SerializeField] private int[] intermissionSwitchMoment;
        [SerializeField] private CinemachineVirtualCamera[] endingVirtualCameras;
        [SerializeField] private int[] endingSwitchMoment;
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
            var main = storm.GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Callback;
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
            if (!softClouds.GetComponent<ParticleSystem>().isPlaying)
            {
                softClouds.GetComponent<ParticleSystem>().Play();
            }
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

        public void ToggleStorm(bool stormActive)
        {
            if (stormActive)
            {
                if (softClouds.GetComponent<ParticleSystem>().isPlaying)
                {
                    softClouds.GetComponent<ParticleSystem>().Stop();
                }
                storm.gameObject.SetActive(true);
            }
            else
            {
                storm.gameObject.SetActive(false);
                softClouds.GetComponent<ParticleSystem>().Play();
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
                    animals.SetActive(true);
                    fogScript.StopFade();
                    fanScript.isActive = true;
                    board.SetActive(false);
                    _transitioning = false;
                }
            }
        }

        public void StartIntermission()
        {
            index = 0;
            ToggleStorm(true);
            _dialoguePresenter.QueueMessages(
                _dialogueGenerationService.SplitTextToInstructionMessages(
                    _dialoguePresenter.GetInstruction("Intermission")));
            _dialoguePresenter.ShowNextMessage();
        }

        public void StartSecondRound()
        {
            ToggleStorm(false);
            GameManager.Instance.ChangeGameState(GameManager.GameState.CollectiveTrade);
        }

        public void StartEnding()
        {
            index = 0;
            _dialoguePresenter.QueueMessages(
                _dialogueGenerationService.SplitTextToInstructionMessages(
                    _dialoguePresenter.GetInstruction("Ending")));
            _dialoguePresenter.ShowNextMessage();
        }
        
        void Update()
        {
            if (_darken)
            {
                light.intensity = Mathf.Lerp(_max, _min, _t);
                _t += 0.5f * Time.deltaTime;
                if (light.intensity == _min) _darken = false;
            }

            if (switchMoment.Length -1 != index && GameManager.Instance.State == GameManager.GameState.Introduction)
            {
                if (switchMoment[index] == _dialoguePresenter.MessagesRemaining() || switchCamera)
                {
                    virtualCameras[index].gameObject.SetActive(false);
                    index++;
                    virtualCameras[index].gameObject.SetActive(true);
                    switchCamera = false;
                }
            }

            if (intermissionSwitchMoment.Length != index && GameManager.Instance.State == GameManager.GameState.Intermission)
            {
                if (intermissionSwitchMoment[index] == _dialoguePresenter.MessagesRemaining() || switchCamera)
                {
                    intermissionVirtualCameras[index].gameObject.SetActive(true);
                    index++;
                    switchCamera = false;
                }
            }
            
            if (endingSwitchMoment.Length != index && GameManager.Instance.State == GameManager.GameState.Ending)
            {
                if (endingSwitchMoment[index] == _dialoguePresenter.MessagesRemaining() || switchCamera)
                {
                    endingVirtualCameras[index].gameObject.SetActive(true);
                    index++;
                    switchCamera = false;
                }
            }
        }
    }
}
