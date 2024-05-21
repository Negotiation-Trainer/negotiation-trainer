using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmountInputField : MonoBehaviour
{
    private TMP_InputField _numberInput;

    private void Start()
    {
        _numberInput = GetComponent<TMP_InputField>();
        _numberInput.onValueChanged.AddListener(ValueChanged);
    }
    
    private void ValueChanged(string txt)
    {
        if (txt.Length > 0 && txt[0] == '-') _numberInput.text = txt.Remove(0, 1);
    }
}
