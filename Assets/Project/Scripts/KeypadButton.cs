using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    [SerializeField] private int buttonValue;
    private TextMeshProUGUI buttonText;
    [SerializeField] private KeypadManager keypadManager;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = buttonValue.ToString();
    }

    public void ButtonPress()
    {
        keypadManager.NumberPressed(buttonValue);
    }
}
