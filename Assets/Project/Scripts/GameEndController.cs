using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the UI for the Game's End screen
/// </summary>
public class GameEndController : MonoBehaviour
{
    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }

    public void OpenForm(string url)
    {
        Application.OpenURL(url);
    }
}
