using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
        buttonPressSfx.Play();

        if (currentPassword.Length < 4)
        {
            currentPassword += value.ToString();
        }

        UpdateDisplay();
    }

    public void CancelPressed()
    {
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
        gameObject.SetActive(false);
        display.text = string.Empty;
    }

    public void HidePanel()
    {
        GameManager.Instance.player_busy = false;
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        GameManager.Instance.player_busy = true;
    }

    private void Update()
    {
        GameManager.Instance.player_busy = true;
    }

    public void EnterPressed()
    {
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
        foreach (var bt in buttonArray.GetComponentsInChildren<Button>())
        {
            bt.enabled = !newState;
        }
    }

    private void Incorrect()
    {
        failSfx.Play();

        display.color = Color.red;
        currentPassword = "----";
        UpdateDisplay();
        StartCoroutine(AutoResetAfterFail());
    }

    private IEnumerator AutoResetAfterFail()
    {
        DisableButtons(true);
        yield return new WaitForSeconds(1);
        currentPassword = string.Empty;
        display.color = Color.white;
        UpdateDisplay();
        DisableButtons(false);
    }

    private IEnumerator AutoHideAfterUnlock()
    {
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
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Animator>().Play("Open");
        interaction.SetActive(false);
    }

    private void GoodPassword()
    {
        successSfx.Play();
        keypadLight.GetComponent<Light2D>().color = Color.green;

        display.color = Color.green;

        currentPassword = "----";
        UpdateDisplay();

        StartCoroutine(AutoHideAfterUnlock());
    }

    private void UpdateDisplay()
    {
        display.text = currentPassword;
    }
}
