using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int[] chaptersSceneIndex;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject helpText;

    public void Start()
    {
        helpText.SetActive(false);

        if (GameSaver.Instance.lastPlayedChapter == -1)
        {
            continueButton.SetActive(false);
        }
    }

    public void Continue()
    {
        LoadScene(chaptersSceneIndex[GameSaver.Instance.lastPlayedChapter]);
    }
    public void LoadScene(int sceneIndex)
    {
        if(sceneIndex < 0)
            sceneIndex = 0;

        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToogleHelp()
    {
        helpText.SetActive(!helpText.activeInHierarchy);
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
