using System;
using System.Collections.Generic;
using Enums;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject inputFallback;
        [SerializeField] private TMP_Dropdown offeringResourceType;
        [SerializeField] private TMP_InputField offeringResourceAmount;
        [SerializeField] private TMP_Dropdown requestingResourceType;
        [SerializeField] private TMP_InputField requestingResourceAmount;
        [SerializeField] private TMP_Dropdown targetTribe;
        [SerializeField] private TMP_Text errorText;
        private TradePresenter _tradePresenter;

        private void Start()
        {
           _tradePresenter = GetComponent<TradePresenter>();
           targetTribe.ClearOptions();
           targetTribe.AddOptions(new List<string>(){GameManager.Instance.Cpu1.Name,GameManager.Instance.Cpu2.Name});
        }

        // Start is called before the first frame update
        public void ToggleInputFallBack()
        {
            inputFallback.SetActive(!inputFallback.activeSelf);
        }

        private void ShowError(string error)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = error;
        }

        private void HideError()
        {
            errorText.gameObject.SetActive(false);
        }

        public void ProposeDeal()
        {
            Tribe originator = GameManager.Instance.Player;
            Tribe target = GetTribeFromDropDown(targetTribe);
            int offeredAmount = Convert.ToInt32(offeringResourceAmount.text);
            int requestedAmount = Convert.ToInt32(requestingResourceAmount.text);
            InventoryItems offeredItem = GetInventoryItemFromDropdown(offeringResourceType);
            InventoryItems requestedItem = GetInventoryItemFromDropdown(requestingResourceType);
            
            if (offeredAmount == 0 || requestedAmount == 0)
            {
                ShowError("you cant offer or request 0 resources");
                return;
            }

            if (originator.Inventory.GetInventoryAmount(offeredItem) < offeredAmount)
            {
                ShowError("you don't have enough resources to offer");
                return;
            }
            
            Trade trade = new Trade(requestedItem, requestedAmount, offeredItem, offeredAmount);
            _tradePresenter.ShowTradeOffer(trade,originator,target);
            HideError();
            ToggleInputFallBack();
        }

        InventoryItems GetInventoryItemFromDropdown(TMP_Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    return InventoryItems.Wood;
                case 1:
                    return InventoryItems.Insulation;
                case 2:
                    return InventoryItems.Stone;
                case 3:
                    return InventoryItems.Fertilizer;
                case 4:
                    return InventoryItems.Lenses;
                case 5:
                    return InventoryItems.Clay;
                case 6:
                    return InventoryItems.Gold;
                case 7:
                    return InventoryItems.Steel;
                default:
                    return InventoryItems.Wood;
            }
        }

        Tribe GetTribeFromDropDown(TMP_Dropdown dropdown)
        {
            if (dropdown.value == 0) return GameManager.Instance.Cpu1;
            return GameManager.Instance.Cpu2;
        }
    }
}
