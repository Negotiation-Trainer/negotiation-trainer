using System;
using System.Collections.Generic;
using ModelLibrary;
using Presenters;
using ServiceLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityHttpClients;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button[] settingsButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button[] unpauseButtons;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button endGame;
    public static GameManager Instance { get; private set; }

    public static AIService aiService { get; private set; } = new();

    public static BackOfficeHttpClient httpClient { get; private set; }
    
    public Tribe Cpu1 { get; private set; }
    public Tribe Cpu2 { get; private set; }
    public Tribe Player { get; private set; }
    public GameState State { get; private set; }

    private InventoryPresenter _inventoryPresenter;
    private ScorePresenter _scorePresenter;
    private InputPresenter _inputPresenter;
    private SettingsPresenter _settingsPresenter;
    private SpeechPresenter _speechPresenter;
    private CutscenePresenter _cutscenePresenter;
    private DialoguePresenter _DialoguePresenter;
    private BuildingPresenter _buildingPresenter;
    private TradePresenter _tradePresenter;
    
    public enum GameState
    {
        AIOptions,
        Start,
        Introduction,
        Trade,
        Intermission,
        CollectiveTrade,
        Ending
    }

    private void Awake()
    {
        Instance = this;
        
        Player = new Tribe("Azari");
        Cpu1 = new Tribe("Beluga");
        Cpu2 = new Tribe("Cinatu");
        
        Player.GoodWill[Cpu1] = 0;
        Player.GoodWill[Cpu2] = 0;

        Cpu1.GoodWill[Player] = 0;
        Cpu1.GoodWill[Cpu2] = 0;

        Cpu2.GoodWill[Player] = 0;
        Cpu2.GoodWill[Cpu1] = 0;
        
        SetPointTables();
        FillInventory();
    }

    private void Start()
    {
        _inventoryPresenter = GetComponent<InventoryPresenter>();
        _scorePresenter = GetComponent<ScorePresenter>();
        _inputPresenter = GetComponent<InputPresenter>();
        _settingsPresenter = GetComponent<SettingsPresenter>();
        _speechPresenter = GetComponent<SpeechPresenter>();
        _cutscenePresenter = GetComponent<CutscenePresenter>();
        _DialoguePresenter = GetComponent<DialoguePresenter>();
        _buildingPresenter = GetComponent<BuildingPresenter>();
        pauseButton.onClick.AddListener(PauseGame);
        foreach (var button in settingsButton)
        {
            button.onClick.AddListener(ShowSettingsMenu);
        }
        foreach (var button in unpauseButtons)
        {
            button.onClick.AddListener(UnpauseGame);
        }
        ChangeGameState(GameState.AIOptions);
    }

    public void EndGame()
    {
        if (State == GameState.Trade)
        {
            ChangeGameState(GameState.Intermission);
            return;
        }

        if (State == GameState.CollectiveTrade)
        {
            ChangeGameState(GameState.Ending);
        }
    }

    private void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    private void ShowSettingsMenu()
    {
        pauseMenu.SetActive(false);
        _settingsPresenter.ShowSettingsMenu(true);
    }
    private void PauseGame()
    {
        ToggleTradeUI(false);
        pauseMenu.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        _speechPresenter.Pause();
    }
    
    private void UnpauseGame()
    {
        _settingsPresenter.ShowSettingsMenu(false);
        pauseMenu.SetActive(false);
        if(State != GameState.Start) pauseButton.gameObject.SetActive(true);
        if(State == GameState.Trade) ToggleTradeUI(true);
        _speechPresenter.Resume();
    }

    private void SetPointTables()
    {
        Player.PointTable = new Dictionary<(InventoryItems, Tribe), int>
        {
            [(InventoryItems.Wood, Player)] = 10,
            [(InventoryItems.Wood, Cpu1)] = -5,
            [(InventoryItems.Wood, Cpu2)] = 5,

            [(InventoryItems.Lenses, Player)] = 10,
            [(InventoryItems.Lenses, Cpu1)] = 5,
            [(InventoryItems.Lenses, Cpu2)] = 0,

            [(InventoryItems.Clay, Player)] = 10,
            [(InventoryItems.Clay, Cpu1)] = 0,
            [(InventoryItems.Clay, Cpu2)] = -5,

            [(InventoryItems.Gold, Player)] = 10,
            [(InventoryItems.Gold, Cpu1)] = 5,
            [(InventoryItems.Gold, Cpu2)] = 0,

            [(InventoryItems.Steel, Player)] = 10,
            [(InventoryItems.Steel, Cpu1)] = 0,
            [(InventoryItems.Steel, Cpu2)] = 0,

            [(InventoryItems.Insulation, Player)] = 10,
            [(InventoryItems.Insulation, Cpu1)] = 5,
            [(InventoryItems.Insulation, Cpu2)] = 5,

            [(InventoryItems.Fertilizer, Player)] = 10,
            [(InventoryItems.Fertilizer, Cpu1)] = 0,
            [(InventoryItems.Fertilizer, Cpu2)] = 0,

            [(InventoryItems.Stone, Player)] = 10,
            [(InventoryItems.Stone, Cpu1)] = 0,
            [(InventoryItems.Stone, Cpu2)] = 5,
        };

        Cpu1.PointTable = new Dictionary<(InventoryItems, Tribe), int>
        {
            [(InventoryItems.Wood, Player)] = 0,
            [(InventoryItems.Wood, Cpu1)] = 10,
            [(InventoryItems.Wood, Cpu2)] = 5,

            [(InventoryItems.Lenses, Player)] = -5,
            [(InventoryItems.Lenses, Cpu1)] = 10,
            [(InventoryItems.Lenses, Cpu2)] = 0,

            [(InventoryItems.Clay, Player)] = 5,
            [(InventoryItems.Clay, Cpu1)] = 10,
            [(InventoryItems.Clay, Cpu2)] = -5,

            [(InventoryItems.Gold, Player)] = 5,
            [(InventoryItems.Gold, Cpu1)] = 10,
            [(InventoryItems.Gold, Cpu2)] = -5,

            [(InventoryItems.Steel, Player)] = 0,
            [(InventoryItems.Steel, Cpu1)] = 10,
            [(InventoryItems.Steel, Cpu2)] = 5,

            [(InventoryItems.Insulation, Player)] = 5,
            [(InventoryItems.Insulation, Cpu1)] = 10,
            [(InventoryItems.Insulation, Cpu2)] = 5,

            [(InventoryItems.Fertilizer, Player)] = 0,
            [(InventoryItems.Fertilizer, Cpu1)] = 10,
            [(InventoryItems.Fertilizer, Cpu2)] = 0,

            [(InventoryItems.Stone, Player)] = 0,
            [(InventoryItems.Stone, Cpu1)] = 10,
            [(InventoryItems.Stone, Cpu2)] = 0,
        };
        
        Cpu2.PointTable = new Dictionary<(InventoryItems, Tribe), int>
        {
            [(InventoryItems.Wood, Player)] = 0,
            [(InventoryItems.Wood, Cpu1)] = -5,
            [(InventoryItems.Wood, Cpu2)] = 10,
            
            [(InventoryItems.Lenses, Player)] = -5,
            [(InventoryItems.Lenses, Cpu1)] = 5,
            [(InventoryItems.Lenses, Cpu2)] = 10,
            
            [(InventoryItems.Clay, Player)] = 5,
            [(InventoryItems.Clay, Cpu1)] = 0,
            [(InventoryItems.Clay, Cpu2)] = 10,
            
            [(InventoryItems.Gold, Player)] = 0,
            [(InventoryItems.Gold, Cpu1)] = 0,
            [(InventoryItems.Gold, Cpu2)] = 10,
            
            [(InventoryItems.Steel, Player)] = -5,
            [(InventoryItems.Steel, Cpu1)] = 0,
            [(InventoryItems.Steel, Cpu2)] = 10,
            
            [(InventoryItems.Insulation, Player)] = 0,
            [(InventoryItems.Insulation, Cpu1)] = -5,
            [(InventoryItems.Insulation, Cpu2)] = 10,
            
            [(InventoryItems.Fertilizer, Player)] = 0,
            [(InventoryItems.Fertilizer, Cpu1)] = 0,
            [(InventoryItems.Fertilizer, Cpu2)] = 10,
            
            [(InventoryItems.Stone, Player)] = 0,
            [(InventoryItems.Stone, Cpu1)] = 5,
            [(InventoryItems.Stone, Cpu2)] = 10,
        };
    }
    
    private void FillInventory()
    {
        Player.Inventory.AddToInventory(InventoryItems.Wood, 6);
        Player.Inventory.AddToInventory(InventoryItems.Lenses, 6);
        Player.Inventory.AddToInventory(InventoryItems.Clay, 1);
        Player.Inventory.AddToInventory(InventoryItems.Gold, 6);
        Player.Inventory.AddToInventory(InventoryItems.Steel, 2);
        Player.Inventory.AddToInventory(InventoryItems.Insulation, 0);
        Player.Inventory.AddToInventory(InventoryItems.Fertilizer, 2);
        Player.Inventory.AddToInventory(InventoryItems.Stone, 2);
        
        Cpu1.Inventory.AddToInventory(InventoryItems.Wood, 4);
        Cpu1.Inventory.AddToInventory(InventoryItems.Lenses, 2);
        Cpu1.Inventory.AddToInventory(InventoryItems.Clay, 4);
        Cpu1.Inventory.AddToInventory(InventoryItems.Gold, 5);
        Cpu1.Inventory.AddToInventory(InventoryItems.Steel, 5);
        Cpu1.Inventory.AddToInventory(InventoryItems.Insulation, 3);
        Cpu1.Inventory.AddToInventory(InventoryItems.Fertilizer, 3);
        Cpu1.Inventory.AddToInventory(InventoryItems.Stone, 6);
        
        Cpu2.Inventory.AddToInventory(InventoryItems.Wood, 1);
        Cpu2.Inventory.AddToInventory(InventoryItems.Lenses, 2);
        Cpu2.Inventory.AddToInventory(InventoryItems.Clay, 6);
        Cpu2.Inventory.AddToInventory(InventoryItems.Gold, 1);
        Cpu2.Inventory.AddToInventory(InventoryItems.Steel, 4);
        Cpu2.Inventory.AddToInventory(InventoryItems.Insulation, 7);
        Cpu2.Inventory.AddToInventory(InventoryItems.Fertilizer, 4);
        Cpu2.Inventory.AddToInventory(InventoryItems.Stone, 5);
    }

    public void ChangeGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.AIOptions:
                HandleAIOptionsState();
                State = newState;
                break;
            case GameState.Start:
                HandleGameStartState();
                State = newState;
                break;
            case GameState.Introduction:
                HandleIntroductionState();
                State = newState;
                break;
            case GameState.Trade:
                HandleTradeState();
                State = newState;
                break;
            case GameState.Intermission:
                HandleIntermissionState();
                State = newState;
                break;
            case GameState.CollectiveTrade:
                HandleTradeState();
                State = newState;
                break;
            case GameState.Ending:
                HandleEndingState();
                State = newState;
                break;
        }
    }

    public void StartIntroduction()
    {
        HandleIntroductionState();
        State = GameState.Introduction;
    }

    public void StartTrade()
    {
        HandleTradeState();
        State = GameState.Trade;
    }

    private void ToggleTradeUI(bool isActive)
    {
        _inventoryPresenter.ShowResourceCard(isActive);
        _scorePresenter.ShowScoreCard(isActive);
        _inputPresenter.ToggleNewOfferButton(isActive);
        _inputPresenter.ToggleTalkButton(isActive);
        _tradePresenter.ToggleCurrentTribe(isActive);
        
    }

    private void HandleEndingState()
    {
        ToggleAIOptions(false); 
        ToggleTradeUI(false);
        pauseButton.gameObject.SetActive(false);
        mainMenu.SetActive(false);
        endGame.gameObject.SetActive(false);
        _cutscenePresenter.ToggleRainbow(true);
        _cutscenePresenter.StartEnding();
    }
    private void HandleIntermissionState()
    {
        endGame.gameObject.SetActive(false);
        ToggleAIOptions(false); 
        ToggleTradeUI(false);
        pauseButton.gameObject.SetActive(false);
        mainMenu.SetActive(false);

        EmptyInventory(Player);
        EmptyInventory(Cpu1);
        EmptyInventory(Cpu2);
        FillInventory();
        
        _scorePresenter.ResetScoreCard();
        _buildingPresenter.ResetBuildings();
        
        Player.Points = 0;
        Cpu1.Points = 0;
        Cpu2.Points = 0;
        
        Player.GoodWill[Cpu1] = 0;
        Player.GoodWill[Cpu2] = 0;

        Cpu1.GoodWill[Player] = 0;
        Cpu1.GoodWill[Cpu2] = 0;

        Cpu2.GoodWill[Player] = 0;
        Cpu2.GoodWill[Cpu1] = 0;
        
        _cutscenePresenter.StartIntermission();
    }

    private void EmptyInventory(Tribe tribe)
    {
        foreach (InventoryItems resource in Enum.GetValues(typeof(InventoryItems)))
        {
            tribe.Inventory.RemoveFromInventory(resource, tribe.Inventory.GetInventoryAmount(resource));
        }
    }

    private void HandleAIOptionsState()
    {
        ToggleAIOptions(true); 
        ToggleTradeUI(false);
        pauseButton.gameObject.SetActive(false);
        mainMenu.SetActive(false);
    }
    
    /// <summary>
    /// Show menu UI
    /// </summary>
    private void HandleGameStartState()
    {
        ToggleTradeUI(false);
        ToggleAIOptions(false);
        pauseButton.gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }


    /// <summary>
    /// play introduction cutscene
    /// </summary>
    private void HandleIntroductionState()
    {
        ToggleTradeUI(false);
        ToggleAIOptions(false);
        pauseButton.gameObject.SetActive(true);
        _cutscenePresenter.StartGame();
    }

    /// <summary>
    /// Show trade UI
    /// </summary>
    private void HandleTradeState()
    {
        ToggleTradeUI(true);
        ToggleAIOptions(false);
        pauseButton.gameObject.SetActive(true);
        endGame.gameObject.SetActive(true);
    }

    #region tempSetting

    [SerializeField] private GameObject aiOptions;
    [SerializeField] private TMP_InputField aiBaseURL;
    [SerializeField] private TMP_InputField aiSessionPassword;
    

    private void ToggleAIOptions(bool isActive)
    {
        aiOptions.SetActive(isActive);
    }

    public void OnAIOkButton()
    {
        var baseUrl = aiBaseURL.text;
        var sessionPassword = aiSessionPassword.text;
        httpClient = new BackOfficeHttpClient(baseUrl, sessionPassword);

        StartCoroutine(httpClient.Authenticate(OnReceiveToken));
    }

    private void OnReceiveToken(string token)
    {
        Debug.Log("Token received: " + token);
        httpClient.SetToken(token);
        
        Debug.Log("Token is set to: " + httpClient.Debug_GetAuth());
        ChangeGameState(GameState.Start);
    }
    

    #endregion
    
}
