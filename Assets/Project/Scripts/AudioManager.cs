using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ambiance;
    
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetAmbience(bool newState)
    {
        if (newState)
        {
            ambiance.Play();
        }
        else
        {
            ambiance.Stop();
        }
    }

}
