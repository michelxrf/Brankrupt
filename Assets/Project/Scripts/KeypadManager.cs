using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    [SerializeField] private string correctPassowrd;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private AudioSource buttonPressSfx;
    [SerializeField] private AudioSource successSfx;
    [SerializeField] private AudioSource failSfx;
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

    private void Start()
    {
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
           Unlock();
        }
        else
        {
            Incorrect();
        }
    }

    private void Incorrect()
    {
        failSfx.Play();

        currentPassword = string.Empty;
        UpdateDisplay();
    }

    private void Unlock()
    {
        AudioManager.Instance.PlayDoor();

        currentPassword = string.Empty;
        UpdateDisplay();

        HidePanel();
    }

    private void UpdateDisplay()
    {
        display.text = currentPassword;
    }
}
