using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// does a script controlled animation of the game's ending cutscene
/// fading the in and out video, text, audio and back button
/// </summary>
public class CutsceneController : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private GameObject goodEndAnim;
    [SerializeField] private GameObject badEndAnim;
    [SerializeField] private TMP_Text goodEndingText;
    [SerializeField] private TMP_Text badEndingText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource goodEndingMend;
    [SerializeField] private AudioSource badEndingMend;

    private bool isGoodEnding = false;
    private AnimatorStateInfo stateInfo;
    private Animator anim;
    private bool finishedAnim = false;

    private void Awake()
    {
        backToMenuButton.SetActive(false);
        goodEndAnim.SetActive(false);
        badEndAnim.SetActive(false);

        goodEndingText.color = new Color(goodEndingText.color.r, goodEndingText.color.g, goodEndingText.color.b, 0f);
        badEndingText.color = new Color(goodEndingText.color.r, goodEndingText.color.g, goodEndingText.color.b, 0f);

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

    private IEnumerator FadeVideoToBlack(Image vp)
    {
        float audioFixStartingVolume = goodEndingMend.volume;

        float transitionAlpha = vp.color.a;
        float startAlpha = 1f;

        for (float t = 0; t < (fadeDuration / 3f); t += Time.deltaTime)
        {
            float normalizedTime = t / (fadeDuration / 3f);
            transitionAlpha = Mathf.Lerp(startAlpha, 0, normalizedTime);

            goodEndingMend.volume = audioFixStartingVolume * transitionAlpha;
            badEndingMend.volume = audioFixStartingVolume * transitionAlpha;

            vp.color = new Color(vp.color.r, vp.color.g, vp.color.b, transitionAlpha);
            yield return null;
        }

        vp.color = new Color(vp.color.r, vp.color.g, vp.color.b, 0f);
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
            goodEndAnim.SetActive(true);
            anim = goodEndAnim.GetComponent<Animator>();
        }
        else
        {
            badEndAnim.SetActive(true);
            anim = badEndAnim.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 1f && !finishedAnim)
        {
            finishedAnim = true;
            OnAnimEnd();
        }
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }

    private void OnAnimEnd()
    {
        if (isGoodEnding)
        {
            goodEndingMend.Play();
            StartCoroutine(FadeVideoToBlack(goodEndAnim.GetComponent<Image>()));
        }
        else
        {
            badEndingMend.Play();
            StartCoroutine(FadeVideoToBlack(badEndAnim.GetComponent<Image>()));
        }
    }
}
