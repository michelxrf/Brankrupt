using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
