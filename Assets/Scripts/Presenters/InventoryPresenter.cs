using System;
using Enums;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class InventoryPresenter: MonoBehaviour
    {
        [SerializeField] private Transform resourceCard;
        [SerializeField] private Transform resourceCardTarget;
        [SerializeField] private float speed;
        private Vector3 _originalPosition;
        private Vector3 _targetPosition;

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
        private bool _transitioning = false;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _player = _gameManager.Player;
            
            _originalPosition = resourceCard.position;
            _targetPosition = _originalPosition;
            
            _player.Inventory.InventoryUpdate += OnInventoryUpdate;
            _player.Inventory.UpdateInventory();
        }

        public void ToggleResourceCard()
        {
            if(_transitioning) return;
            if (_targetPosition.Compare(_originalPosition, 100))
            {
                _targetPosition = resourceCardTarget.position;
                _transitioning = true;
            }
            else
            {
                _targetPosition = _originalPosition;
                _transitioning = true;
            }
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

        private void FixedUpdate()
        {
            if (_transitioning)
            {
                resourceCard.position = Vector3.MoveTowards(resourceCard.position, _targetPosition, speed);
                if (resourceCard.position.Compare(_targetPosition, 100)) _transitioning = false;
            }
        }
    }
}