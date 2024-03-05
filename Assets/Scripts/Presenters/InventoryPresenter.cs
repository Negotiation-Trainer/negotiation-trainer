using System;
using Enums;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class InventoryPresenter: MonoBehaviour
    {
        [SerializeField] private TMP_Text player;
        [SerializeField] private TMP_Text cpu1;
        [SerializeField] private TMP_Text cpu2;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            player.text = _gameManager.Player.Inventory.ToString();
            cpu1.text = _gameManager.Cpu1.Inventory.ToString();
            cpu2.text = _gameManager.Cpu2.Inventory.ToString();
            
            _gameManager.Player.Inventory.InventoryUpdate += OnInventoryUpdate;
            _gameManager.Cpu1.Inventory.InventoryUpdate += OnInventoryUpdate;
            _gameManager.Cpu2.Inventory.InventoryUpdate += OnInventoryUpdate;
        }
        
        private void OnInventoryUpdate(object sender, EventArgs eventArgs)
        {
            player.text = _gameManager.Player.Inventory.ToString();
            cpu1.text = _gameManager.Cpu1.Inventory.ToString();
            cpu2.text = _gameManager.Cpu2.Inventory.ToString();
        }
    }
}