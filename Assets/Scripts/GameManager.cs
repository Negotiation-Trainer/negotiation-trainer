using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using Enums;
using Models;
using Presenters;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Tribe Cpu1 { get; private set; }
    public Tribe Cpu2 { get; private set; }
    public Tribe Player { get; private set; }

    private void Awake()
    {
        Instance = this;

        Player = new Tribe("Azari");
        Cpu1 = new Tribe("Beluga");
        Cpu2 = new Tribe("Cinatu");
        
        SetPointTables();
        FillInventory();
        
        //intro scene start button
        startButton.onClick.AddListener(StartGame);
        _stormParticleSystem = storm.GetComponent<ParticleSystem>();
        _dialoguePresenter = GetComponent<DialoguePresenter>();
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

    #region IntroScene

    [SerializeField] private GameObject island;
    [SerializeField] private GameObject board;
    [SerializeField] private Transform endMarker;
    [SerializeField] private float speed = 0.5F;
    [SerializeField] private float delay = 3.5F;

    [SerializeField] private GameObject softClouds;
    [SerializeField] private GameObject storm;
    private ParticleSystem _stormParticleSystem;

    [SerializeField] private Button startButton;
    private DialoguePresenter _dialoguePresenter;
    private GameState _gameState = GameState.Start;

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private GameObject startCamera;

    private enum GameState
    {
        Start,
        MovingIsland,
        GeneralInstruction
    }

    private void StartGame()
    {
        storm.SetActive(true);
        Invoke(nameof(MoveIsland), delay);
        startButton.gameObject.SetActive(false);
    }
    
    private void MoveIsland()
    {
        _gameState = GameState.MovingIsland;
    }

    private void StartInstruction()
    {
        _dialoguePresenter.StartGeneralInstruction();
        softClouds.SetActive(true);
    }
    
    void FixedUpdate()
    {
        if(_gameState == GameState.MovingIsland)
        {
            island.transform.position = Vector3.MoveTowards(island.transform.position, endMarker.position, speed);
            if (island.transform.position == endMarker.position && _stormParticleSystem.isStopped)
            {
                startCamera.SetActive(false);
                playableDirector.gameObject.SetActive(true);
                playableDirector.Play();
                _gameState = GameState.GeneralInstruction;
                board.SetActive(false);
                storm.SetActive(false);
                StartInstruction();
            }
        }
        
    }

    #endregion
}
