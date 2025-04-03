using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private VideoPlayer goodEndClip;
    [SerializeField] private VideoPlayer badEndClip;
    [SerializeField] private NPCConversation goodEndDialog;
    [SerializeField] private NPCConversation badEndDialog;
    [SerializeField] private TMP_Text goodEndingText;
    [SerializeField] private TMP_Text badEndingText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource goodEndingMend;
    [SerializeField] private AudioSource badEndingMend;

    public bool isGoodEnding = false;

    private void Awake()
    {
        goodEndClip.loopPointReached += OnVideoEnd;
        badEndClip.loopPointReached += OnVideoEnd;

        goodEndClip.Prepare();
        badEndClip.Prepare();

        backToMenuButton.SetActive(false);
        goodEndingText.color = new Color(goodEndingText.color.r, goodEndingText.color.g, goodEndingText.color.b, 0f);
        badEndingText.color = new Color(goodEndingText.color.r, goodEndingText.color.g, goodEndingText.color.b, 0f);

        // destroi os singletons que controlam o jogo
        AudioManager.Instance.StopAmbiance();

        isGoodEnding = GameManager.Instance.isGoodEnding;
    }

    private IEnumerator FadeFromBlack(TMP_Text textBox)
    {
        bgm.Play();

        Color panelColor = textBox.color;
        float startAlpha = 0f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / (fadeDuration);
            panelColor.a = Mathf.Lerp(startAlpha, 1, normalizedTime);
            textBox.color = panelColor;
            yield return null;
        }

        backToMenuButton.SetActive(true);
    }

    private IEnumerator FadeVideoToBlack(VideoPlayer vp)
    {
        float audioFixStartingVolume = goodEndingMend.volume;

        float transitionAlpha = vp.targetCameraAlpha;
        float startAlpha = 1f;

        for (float t = 0; t < (fadeDuration / 3f); t += Time.deltaTime)
        {
            float normalizedTime = t / (fadeDuration / 3f);
            transitionAlpha = Mathf.Lerp(startAlpha, 0, normalizedTime);

            goodEndingMend.volume = audioFixStartingVolume * transitionAlpha;
            badEndingMend.volume = audioFixStartingVolume * transitionAlpha;

            vp.targetCameraAlpha = transitionAlpha;
            yield return null;
        }

        vp.targetCameraAlpha = 0f;
        badEndingMend.volume = 0f;
        goodEndingMend.volume = 0f;

        if (isGoodEnding)
        {
            StartCoroutine(FadeFromBlack(goodEndingText));
        }
        else
        {
            StartCoroutine(FadeFromBlack(badEndingText));
        }
    }

    void Start()
    {
        if (isGoodEnding)
        {
            goodEndClip.Play();
        }
        else
        {
            badEndClip.Play();
        }
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (isGoodEnding)
        {
            goodEndingMend.Play();
            StartCoroutine(FadeVideoToBlack(goodEndClip));
        }
        else
        {
            badEndingMend.Play();
            StartCoroutine(FadeVideoToBlack(badEndClip));
        }
    }
}
