using ModelLibrary;
using Presenters;
using ServiceLibrary;
using UnityEngine;

public class DebugPresenter : MonoBehaviour
{
    private bool _enabeld = false;
    private GameManager _gameManager;
    private DialoguePresenter _dialoguePresenter;
    private SpeechPresenter _speechPresenter;
    private TradePresenter _tradePresenter;
    private InputPresenter _inputPresenter;

    [SerializeField] private InventoryItems debugRequestedItem = InventoryItems.Wood;
    [SerializeField] private int debugRequestedAmount = 2;
    [SerializeField] private InventoryItems debugOfferedItem = InventoryItems.Steel;
    [SerializeField] private int debugOfferedAmount = 2;

    private DialogueGenerationService _dialogueGenerationService;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _dialoguePresenter = GetComponent<DialoguePresenter>();
        _speechPresenter = GetComponent<SpeechPresenter>();
        _tradePresenter = GetComponent<TradePresenter>();
        _inputPresenter = GetComponent<InputPresenter>();

        StartServices();
    }
    
    void StartServices()
    {
        _dialogueGenerationService = new DialogueGenerationService();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _enabeld = !_enabeld;
        }
    }

    void OnGUI()
    {
        if (_enabeld)
        {
            GUILayout.Label("Game Manger");
            if (GUILayout.Button("Start state"))
            {
                _gameManager.ChangeGameState(GameManager.GameState.Start);
            }
            if (GUILayout.Button("Introduction state"))
            {
                _gameManager.ChangeGameState(GameManager.GameState.Introduction);
            }
            if (GUILayout.Button("Trade state"))
            {
               _gameManager.ChangeGameState(GameManager.GameState.Trade);
            }
            if (GUILayout.Button("Intermission state"))
            {
                _gameManager.ChangeGameState(GameManager.GameState.Intermission);
            }
            if (GUILayout.Button("Ending state"))
            {
                _gameManager.ChangeGameState(GameManager.GameState.Ending);
            }
            if (GUILayout.Button("Build wood"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Wood,10);
            }
            if (GUILayout.Button("Build stone"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Stone,10);
            }
            if (GUILayout.Button("Build lenses"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Lenses,10);
            }
            if (GUILayout.Button("Build gold"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Gold,10);
            }
            if (GUILayout.Button("Build steel"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Steel,10);
            }
            if (GUILayout.Button("Build clay"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Clay,10);
            }
            if (GUILayout.Button("Build insulation"))
            {
                _gameManager.Player.Inventory.AddToInventory(InventoryItems.Insulation,10);
            }
            if (_dialoguePresenter)
            {
                GUILayout.Label("Dialogue");
                if (GUILayout.Button("Tribe A"))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToInstructionMessages(
                            _dialoguePresenter.GetInstruction("a")));
                    _dialoguePresenter.ShowNextMessage();
                }

                if (GUILayout.Button("Tribe B"))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToInstructionMessages(
                            _dialoguePresenter.GetInstruction("b")));
                    _dialoguePresenter.ShowNextMessage();
                }

                if (GUILayout.Button("Tribe C"))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToInstructionMessages(
                            _dialoguePresenter.GetInstruction("c")));
                    _dialoguePresenter.ShowNextMessage();
                }

                if (GUILayout.Button("General"))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToInstructionMessages(
                            _dialoguePresenter.GetInstruction("general")));
                    _dialoguePresenter.ShowNextMessage();
                }

                if (GUILayout.Button("Dialogue"))
                {
                    _dialoguePresenter.QueueMessages(
                        _dialogueGenerationService.SplitTextToDialogueMessages(
                            _dialoguePresenter.GetInstruction("general"),
                            "DebugTribe"));
                    _dialoguePresenter.ShowNextMessage();
                }
            }

            if (_speechPresenter)
            {
                GUILayout.Label("Speech");
                if (GUILayout.Button("Speak"))
                {
                    _speechPresenter.Speak(
                        "Hello this is a debug text to test the speech to text capability of connor's paradise");
                }

                if (GUILayout.Button("Stop speak"))
                {
                    _speechPresenter.StopSpeaking();
                }
                
                if (GUILayout.Button("Start STT"))
                {
                    _speechPresenter.StartRecognition();
                }
    
                if (GUILayout.Button("Voices"))
                {
                   Debug.Log(_speechPresenter.PossibleVoices());
                }
                
                if (GUILayout.Button("Volume"))
                {
                    Debug.Log(_speechPresenter.GetSpeechVolume());
                }
                
                if (GUILayout.Button("Rate"))
                {
                    Debug.Log(_speechPresenter.GetSpeechRate());
                }

            }

            if (_tradePresenter)
            {
                GUILayout.Label("Trade");
                if (GUILayout.Button("Show trade offer to player"))
                {
                    var trade = new Trade(debugRequestedItem, debugRequestedAmount, debugOfferedItem,
                        debugOfferedAmount, _gameManager.Player.Name, _gameManager.Cpu1.Name);
                    _tradePresenter.ShowTradeOffer(trade, _gameManager.Cpu1, _gameManager.Player);
                }

                if (GUILayout.Button("Show trade offer from player"))
                {
                    var trade = new Trade(debugRequestedItem, debugRequestedAmount, debugOfferedItem,
                        debugOfferedAmount, _gameManager.Cpu1.Name, _gameManager.Player.Name);
                    _tradePresenter.ShowTradeOffer(trade, _gameManager.Player, _gameManager.Cpu1);
                }
                
                if (GUILayout.Button("Create trade"))
                {
                    _tradePresenter.CreateNewTrade(_gameManager.Cpu1, _gameManager.Cpu2);
                }
            }

            if (_inputPresenter)
            {
                GUILayout.Label("Fallback");
                if (GUILayout.Button("Toggle Fallback"))
                {
                    _inputPresenter.ToggleInputFallBack(true);
                }
            }
        }
    }
}
