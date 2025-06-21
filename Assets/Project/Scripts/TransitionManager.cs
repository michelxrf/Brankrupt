using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This Singleton is meant to allow for the player character to transition between scenes
/// It controls a fade effect and saves the keeps the destination position
/// so the player is correctly positioned acording to the transition's desired pos
/// </summary>
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }
    public Vector2 transitionToPosition;
    public bool has_transitioned = false;
    [SerializeField] private Image fadePanel;
    [SerializeField] float fadeDuration = 1f;

    private void Awake()
    {
        // sets up as Singleton

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        // animates an effect of fading from a black screen

        Color panelColor = fadePanel.color;
        float startAlpha = 1f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            panelColor.a = Mathf.Lerp(startAlpha, 0, normalizedTime);
            fadePanel.color = panelColor;
            yield return null;
        }

        panelColor.a = 0;
        fadePanel.color = panelColor;
    }

    private IEnumerator FadeToBlack()
    {
        // animates the VFX of fading to a black screen

        Color panelColor = fadePanel.color;
        float startAlpha = 0f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            panelColor.a = Mathf.Lerp(startAlpha, 1, normalizedTime);
            fadePanel.color = panelColor;
            yield return null;
        }

        panelColor.a = 1;
        fadePanel.color = panelColor;
    }

    public void TransitionTo(int scene_index, Vector2 position)
    {
        // saves the position the player will be placed once they transition to another scene

        transitionToPosition = position;
        has_transitioned = true;
        StartCoroutine(TransitionTo(scene_index));
    }

    public IEnumerator TransitionTo(int scene_index)
    {
        // does the actual transitioning

        GameManager.Instance.is_paused = false;
        GameManager.Instance.player_busy = false;

        yield return FadeToBlack();

        if (scene_index == 0)
            Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(scene_index);

        StartCoroutine(FadeFromBlack());
    }
}
