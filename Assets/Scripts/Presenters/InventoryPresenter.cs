using System;
using ModelLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class InventoryPresenter: MonoBehaviour
    {
        [SerializeField] private Transform resourceCard;
        [SerializeField] private TMP_Text wood;
        [SerializeField] private TMP_Text insulation;
        [SerializeField] private TMP_Text stone;
        [SerializeField] private TMP_Text fertilizer;
        [SerializeField] private TMP_Text lenses;
        [SerializeField] private TMP_Text clay;
        [SerializeField] private TMP_Text gold;
        [SerializeField] private TMP_Text steel;

        private GameManager _gameManager;
        private Tribe _player;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _player = _gameManager.Player;
            
            _player.Inventory.InventoryUpdate += OnInventoryUpdate;
            _player.Inventory.UpdateInventory();
        }

        public void ShowResourceCard(bool isActive)
        {
            resourceCard.gameObject.SetActive(isActive);
        }
        
        private void OnInventoryUpdate(object sender, EventArgs eventArgs)
        {
            wood.text = _player.Inventory.GetInventoryAmount(InventoryItems.Wood).ToString();
            insulation.text = _player.Inventory.GetInventoryAmount(InventoryItems.Insulation).ToString();
            stone.text = _player.Inventory.GetInventoryAmount(InventoryItems.Stone).ToString();
            fertilizer.text = _player.Inventory.GetInventoryAmount(InventoryItems.Fertilizer).ToString();
            lenses.text = _player.Inventory.GetInventoryAmount(InventoryItems.Lenses).ToString();
            clay.text = _player.Inventory.GetInventoryAmount(InventoryItems.Clay).ToString();
            gold.text = _player.Inventory.GetInventoryAmount(InventoryItems.Gold).ToString();
            steel.text = _player.Inventory.GetInventoryAmount(InventoryItems.Steel).ToString();
        }
    }
}