using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// controls the keypad located in the security room
/// it controls the button presses and all it's logic
/// </summary>
public class KeypadManager : MonoBehaviour
{
    [SerializeField] private string correctPassowrd;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private AudioSource buttonPressSfx;
    [SerializeField] private AudioSource successSfx;
    [SerializeField] private AudioSource failSfx;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject buttonArray;
    [SerializeField] private GameObject interaction;
    [SerializeField] private GameObject keypadLight;
    private string currentPassword = string.Empty;

    public void NumberPressed(int value)
    {
        // register the number pressed on the keypad

        buttonPressSfx.Play();

        if (currentPassword.Length < 4)
        {
            currentPassword += value.ToString();
        }

        UpdateDisplay();
    }

    public void CancelPressed()
    {
        // register the "clear" button pressed

        buttonPressSfx.Play();

        if (currentPassword.Length > 0)
        {
            currentPassword = string.Empty;
            UpdateDisplay();
        }
        else
        {
            HidePanel();
        }

        
    }

    private void Awake()
    {
        // ensures its UI is hidden on gamestart

        gameObject.SetActive(false);
        display.text = string.Empty;
    }

    public void HidePanel()
    {
        // used to close the keypad screen

        GameManager.Instance.player_busy = false;
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        // shows the keypad

        gameObject.SetActive(true);
        GameManager.Instance.player_busy = true;
    }

    private void Update()
    {
        // forces the player to stay locked in the interaction
        // necessary due to a conflict with dialog end

        GameManager.Instance.player_busy = true;
    }

    public void EnterPressed()
    {
        // handles the "enter" button press

        buttonPressSfx.Play();

        if (currentPassword.Length != 4)
        {
            Incorrect();
            return;
        }

        if (currentPassword == correctPassowrd)
        {
           GoodPassword();
        }
        else
        {
            Incorrect();
        }
    }

    private void DisableButtons(bool newState)
    {
        // used to temporarily disable the keypad input
        // and allow result fx to be shown undisturbed

        foreach (var bt in buttonArray.GetComponentsInChildren<Button>())
        {
            bt.enabled = !newState;
        }
    }

    private void Incorrect()
    {
        // handles the incorrect password inserted
        // like an "access denied"

        failSfx.Play();

        display.color = Color.red;
        currentPassword = "----";
        UpdateDisplay();
        StartCoroutine(AutoResetAfterFail());
    }

    private IEnumerator AutoResetAfterFail()
    {
        // automatically resets the keypad after an incorrect input

        DisableButtons(true);
        yield return new WaitForSeconds(1);
        currentPassword = string.Empty;
        display.color = Color.white;
        UpdateDisplay();
        DisableButtons(false);
    }

    private IEnumerator AutoHideAfterUnlock()
    {
        // automatically hides the keypad after the "access granted" FX is shown

        DisableButtons(true);
        yield return new WaitForSeconds(1);
        currentPassword = string.Empty;
        display.color = Color.white;
        UpdateDisplay();
        Unlock();
        HidePanel();
        DisableButtons(false);
    }

    private void Unlock()
    {
        // unlocks the door assossiated with the keypad

        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Animator>().Play("Open");
        interaction.GetComponent<Interaction>().disabled = true;
    }

    private void GoodPassword()
    {
        // handles the good password being inserted

        successSfx.Play();
        keypadLight.GetComponent<Light2D>().color = Color.green;

        display.color = Color.green;

        currentPassword = "----";
        UpdateDisplay();

        GameManager.Instance.ChangeObjective("Conserte o disjuntor e acesse as cameras.");

        StartCoroutine(AutoHideAfterUnlock());
    }

    private void UpdateDisplay()
    {
        // calls for updating the visual rep of the passowrd on the Ui

        display.text = currentPassword;
    }
}
