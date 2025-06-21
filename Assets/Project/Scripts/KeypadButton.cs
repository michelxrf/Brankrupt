using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// handles the button press of a single button on the keypad
/// </summary>

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
