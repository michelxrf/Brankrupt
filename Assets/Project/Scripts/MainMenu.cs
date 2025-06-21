using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// controls the behavior of the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] int[] chaptersSceneIndex;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject helpText;

    public void Start()
    {
        // playss theme song and sets up the menu UI

        AudioManager.Instance.PlayTheme();

        helpText.SetActive(false);

        if (GameSaver.Instance.lastPlayedChapter == -1)
        {
            continueButton.SetActive(false);
        }
    }

    public void Continue()
    {
        // loads the scene of the last game saved

        LoadScene(chaptersSceneIndex[GameSaver.Instance.lastPlayedChapter]);
    }
    public void LoadScene(int sceneIndex)
    {
        // does the acutal scene loading

        if(sceneIndex < 0)
            sceneIndex = 0;

        if (sceneIndex != 19)
        {
            AudioManager.Instance.StopTheme();
            AudioManager.Instance.PlayAmbiance();
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToogleHelp()
    {
        // toggles the game's help text

        helpText.SetActive(!helpText.activeInHierarchy);
    }
}
