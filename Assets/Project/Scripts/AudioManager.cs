using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambiance;
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

}
