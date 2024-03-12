using System;
using Enums;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class InventoryPresenter : MonoBehaviour
    {
        [Header("Tribe debug info")] [SerializeField]
        private TMP_Text player;

        [SerializeField] private TMP_Text cpu1;
        [SerializeField] private TMP_Text cpu2;

        [Header("Inventory")] [SerializeField] private TMP_Text inventoryWood;
        [SerializeField] private TMP_Text inventoryLenses;
        [SerializeField] private TMP_Text inventoryClay;
        [SerializeField] private TMP_Text inventoryGold;
        [SerializeField] private TMP_Text inventorySteel;
        [SerializeField] private TMP_Text inventoryInsulation;
        [SerializeField] private TMP_Text inventoryFertilizer;
        [SerializeField] private TMP_Text inventoryStone;


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
            
            UpdateInventoryLabels();
        }

        private void OnInventoryUpdate(object sender, EventArgs eventArgs)
        {
            player.text = _gameManager.Player.Inventory.ToString();
            cpu1.text = _gameManager.Cpu1.Inventory.ToString();
            cpu2.text = _gameManager.Cpu2.Inventory.ToString();
            
            UpdateInventoryLabels();
        }

        private void UpdateInventoryLabels()
        {
            setTextLabel(inventoryWood, InventoryItems.Wood);
            setTextLabel(inventoryLenses, InventoryItems.Lenses);
            setTextLabel(inventoryClay, InventoryItems.Clay);
            setTextLabel(inventoryGold, InventoryItems.Gold);
            setTextLabel(inventorySteel, InventoryItems.Steel);
            setTextLabel(inventoryInsulation, InventoryItems.Insulation);
            setTextLabel(inventoryFertilizer, InventoryItems.Fertilizer);
            setTextLabel(inventoryStone, InventoryItems.Stone);
        }

        private void setTextLabel(TMP_Text label, InventoryItems item)
        {
            label.text = _gameManager.Player.Inventory.GetInventoryAmount(item).ToString();
        }
    }
}