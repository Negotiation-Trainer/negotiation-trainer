using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using ModelLibrary;
using Presenters;
using ServiceLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Tribe Cpu1 { get; private set; }
    public Tribe Cpu2 { get; private set; }
    public Tribe Player { get; private set; }
    public GameState State { get; private set; }

    private InventoryPresenter _inventoryPresenter;
    private ScorePresenter _scorePresenter;
    private InputPresenter _inputPresenter;
    public AIService AIService;
    
    public enum GameState
    {
        Start,
        Introduction,
        Trade
    }

    private void Awake()
    {
        Instance = this;

        Player = new Tribe("Azari");
        Cpu1 = new Tribe("Beluga");
        Cpu2 = new Tribe("Cinatu");

        Player.GoodWill = new Dictionary<Tribe, int>();
        Cpu1.GoodWill = new Dictionary<Tribe, int>();
        Cpu2.GoodWill = new Dictionary<Tribe, int>();
        
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
        Debug.Log("Starting");
        Debug.Log(webResponse);
        StartCoroutine(
            Post("https://negotiation-game.azurewebsites.net/api/v1/authenticate", new Dictionary<string, string>(), new SessionPassword("1G6Y")
            , (response) => { Debug.Log("callbackResponse: " + response); }));
        Debug.Log("Done");
        Debug.Log(webResponse);
        ChangeGameState(GameState.Start);
    }


    IEnumerator GetSessionToken()
    {
        string jsonBody = JsonUtility.ToJson(new SessionPassword("1G6Y"));
        using (UnityWebRequest www = UnityWebRequest.Post("https://negotiation-game.azurewebsites.net/api/v1/authenticate", jsonBody, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    
    

    public string webResponse = "not set yet";

    IEnumerator Post<T>(string pathUrl, Dictionary<string, string> headers, T body, Action<string> callback = null)
    {
        string jsonBody = JsonUtility.ToJson(body) ?? "{}";
        
        using (UnityWebRequest wr = UnityWebRequest.Post($"{pathUrl}", jsonBody, "application/json"))
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                wr.SetRequestHeader(header.Key, header.Value);
            }
            
            yield return wr.SendWebRequest();
            
            if (wr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(wr.error);
                Debug.Log(wr.downloadHandler.text);
            }
            else
            {
                Debug.Log(wr.downloadHandler.text);
            }
            
            webResponse = wr.downloadHandler.text;
            callback?.Invoke(wr.downloadHandler.text);
        }
    }
    
    protected string Post<T>(string pathUrl, string bla, Dictionary<string, string> headers, T body)
    {
        // Add the body to the request when it is not null
        string bodyJson = body != null ? JsonUtility.ToJson(body) : "{}";
        
        Debug.Log(bodyJson);


        using UnityWebRequest request = UnityWebRequest.Post($"https://negotiation-game.azurewebsites.net/api/v1/{pathUrl}", bodyJson, "application/json");
        
        
        
        foreach (KeyValuePair<string, string> header in headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }
                
        var operation = request.SendWebRequest();

        // Wait for the request to complete
        while (!operation.isDone) {Debug.Log("waiting"); }
        
        Debug.Log(request.result);
        Debug.Log(request.error);
        Debug.Log(request.downloadHandler.text);
        
        var test = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);
        
        Debug.Log(test);
        Debug.Log(test.token);
        
        string resultText;
        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                resultText = request.downloadHandler.text;
                break;
            case UnityWebRequest.Result.ConnectionError:
                resultText = $"Connection Error: {request.error}";
                break;
            case UnityWebRequest.Result.ProtocolError:
                resultText = $"Protocol Error: {request.error}";
                break;
            default:
                resultText = "Unknown error";
                break;
        }

        return resultText;
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
        }
    }

    private void ToggleTradeUI(bool isActive)
    {
        _inventoryPresenter.ShowResourceCard(isActive);
        _scorePresenter.ShowScoreCard(isActive);
        _inputPresenter.ToggleNewOfferButton(isActive);
        _inputPresenter.ToggleTalkButton(isActive);
    }

    /// <summary>
    /// Show menu UI
    /// </summary>
    private void HandleGameStartState()
    {
        ToggleTradeUI(false);
        ToggleAIOptions(true);
    }


    /// <summary>
    /// play introduction cutscene
    /// </summary>
    private void HandleIntroductionState()
    {
        ToggleTradeUI(false);
        ToggleAIOptions(false);
    }

    /// <summary>
    /// Show trade UI
    /// </summary>
    private void HandleTradeState()
    {
        ToggleTradeUI(true);
        ToggleAIOptions(false);
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
        AIService = new AIService(baseUrl,sessionPassword);
        ChangeGameState(GameState.Trade);
    }
    

    #endregion
    
    private class SessionPassword
    {
        public string sessionPassword;
        public SessionPassword(string sessionPassword)
        {
            this.sessionPassword = sessionPassword;
        }
    }

    private class TokenResponse
    {
        public string token;
    }

}
