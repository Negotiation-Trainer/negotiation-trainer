using System;
using System.Collections.Generic;
using ModelLibrary;
using TMPro;
using UnityEngine;

namespace Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject newOfferButton;
        [SerializeField] private GameObject inputFallback;
        [SerializeField] private TMP_Dropdown offeringResourceType;
        [SerializeField] private TMP_InputField offeringResourceAmount;
        [SerializeField] private TMP_Dropdown requestingResourceType;
        [SerializeField] private TMP_InputField requestingResourceAmount;
        [SerializeField] private TMP_Dropdown targetTribe;
        [SerializeField] private TMP_Text errorText;
        private TradePresenter _tradePresenter;
        private SettingsPresenter _settingsPresenter;

        private void Start()
        {
            _settingsPresenter = GetComponent<SettingsPresenter>();
           _tradePresenter = GetComponent<TradePresenter>();
           targetTribe.ClearOptions();
           targetTribe.AddOptions(new List<string>(){GameManager.Instance.Cpu1.Name,GameManager.Instance.Cpu2.Name});
        }

        public void ToggleNewOfferButton(bool isActive)
        {
            newOfferButton.SetActive(isActive && _settingsPresenter.fallbackEnabled);
        }
        public void ToggleInputFallBack(bool isActive)
        {
            inputFallback.SetActive(isActive);
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

        private bool CheckForInputErrors(Trade trade, Tribe originator)
        {
            if (trade.OfferedAmount == 0 || trade.RequestedAmount == 0)
            {
                ShowError("you can not offer or request 0 resources");
                return false;
            }
            
            if (originator.Inventory.GetInventoryAmount(trade.OfferedItem) < trade.OfferedAmount)
            {
                ShowError("you don't have enough resources to offer");
                return false;
            }
            
            if (trade.OfferedItem == trade.RequestedItem)
            {
                ShowError("Offered resource can not be the same as the requested resource");
                return false;
            }
            
            return true;
        }

        public void ShowProposal()
        {
            ToggleNewOfferButton(false);
            ToggleInputFallBack(true);
        }

        public void DiscardDeal()
        {
            HideError();
            ToggleInputFallBack(false);
            ToggleNewOfferButton(true);
        }
        
        public void ProposeDeal()
        {
            Tribe originator = GameManager.Instance.Player;
            Tribe target = GetTribeFromDropDown(targetTribe);
            int offeredAmount = Convert.ToInt32(offeringResourceAmount.text);
            int requestedAmount = Convert.ToInt32(requestingResourceAmount.text);
            InventoryItems offeredItem = GetInventoryItemFromDropdown(offeringResourceType);
            InventoryItems requestedItem = GetInventoryItemFromDropdown(requestingResourceType);
            Trade trade = new Trade(requestedItem, requestedAmount, offeredItem, offeredAmount,target.Name , originator.Name);
            
            if (!CheckForInputErrors(trade, originator)) return;
            HideError();
            
            _tradePresenter.ShowTradeOffer(trade,originator,target);
            ToggleInputFallBack(false);
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
