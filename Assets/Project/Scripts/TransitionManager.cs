using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }
    public Vector2 transitionToPosition;
    public bool has_transitioned = false;

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

    public void TransitionTo(int scene_index, Vector2 position)
    {
        transitionToPosition = position;
        has_transitioned = true;
        SceneManager.LoadScene(scene_index);
        GameManager.Instance.is_paused = false;
        GameManager.Instance.player_busy = false;
    }

    public void TransitionTo(int scene_index)
    {
        Debug.Log("Transition Called");
        SceneManager.LoadScene(scene_index);
        GameManager.Instance.is_paused = false;
        GameManager.Instance.player_busy = false;
    }
}
