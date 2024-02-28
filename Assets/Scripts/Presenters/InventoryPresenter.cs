using System;
using Enums;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Presenters
{
    public class InventoryPresenter: MonoBehaviour
    {
        private Inventory _inventory; 
        
        private void Start()
        {
            _inventory = new Inventory();
        }
        
        
    }
}