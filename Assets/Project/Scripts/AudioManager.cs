using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ---
// DISCLAIMER
// Many audio sources would be much better if they were part of the object they belong to
// like the doors should be part of the transitions
// but I decided to centralize them in this singleton for easier access of our audio designer
// we didn`t had a picked audio designer but I knew it was going to be someone with almost no Unity experience
// so I intended to make it the easiest as possible to slot in the audios
// ---
/// <summary>
/// Controls all the audio calls in game
/// it was made a singleton so the ambience would keep it`s track progress between scenes
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambiance;
    [SerializeField] private AudioSource menuTheme;
    [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource lowSanity;
    [SerializeField] private float lowSanityInitVolume;
    
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            lowSanityInitVolume = lowSanity.volume;
            lowSanity.volume = 0f;
        }
    }
    public void PlayDoor()
    {
        doorOpen.Play();
    }

    public void GameOver()
    {
        ambiance.Stop();
        lowSanity.Stop();
    }

    public void Door()
    {
        doorOpen.Play();
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            lowSanity.volume = (1 - GameManager.Instance.currentSanityLevel / GameManager.Instance.maxSanityLevel) * lowSanityInitVolume;
        }
        
    }

    private void OnDestroy()
    {
        ambiance.Stop();
        lowSanity.Stop();
    }

    public void StopAmbiance()
    {
        ambiance.Stop();
    }

    public void PlayAmbiance()
    {
        ambiance.Play();
    }

    public void PlayTheme()
    {
        if (!menuTheme.isPlaying)
            menuTheme.Play();
    }

    public void StopTheme()
    {
        Debug.Log("Stop");
        menuTheme.Stop();
    }
}
