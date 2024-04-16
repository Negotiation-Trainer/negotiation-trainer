using System;
using System.Collections.Generic;
using ModelLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
    public class InputPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject talkButton;
        [SerializeField] private GameObject newOfferButton;
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

        public void ToggleTalkButton(bool isActive)
        {
            talkButton.SetActive(false);
        }
        public void ToggleNewOfferButton(bool isActive)
        {
            newOfferButton.SetActive(isActive);
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

        private bool CheckForInputErrors(Trade trade, Tribe originator, Tribe target)
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
            ToggleTalkButton(false);
            ToggleInputFallBack(true);
        }

        public void DiscardDeal()
        {
            ToggleInputFallBack(false);
            ToggleTalkButton(true);
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
            Trade trade = new Trade(requestedItem, requestedAmount, offeredItem, offeredAmount);
            
            if (!CheckForInputErrors(trade, originator, target)) return;
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
