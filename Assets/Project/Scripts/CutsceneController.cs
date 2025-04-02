using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private VideoPlayer goodEndClip;
    [SerializeField] private VideoPlayer badEndClip;
    [SerializeField] private NPCConversation goodEndDialog;
    [SerializeField] private NPCConversation badEndDialog;

    private bool isGoodEnding = false;

    private void Awake()
    {
        // destroi os singletons que controlam o jogo
        Destroy(AudioManager.Instance.gameObject);

        isGoodEnding = GameManager.Instance.isGoodEnding;
        Destroy(GameManager.Instance.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isGoodEnding)
        {
            goodEndClip.Play();
            ConversationManager.Instance.StartConversation(goodEndDialog, null);
        }
        else
        {
            badEndClip.Play();
            ConversationManager.Instance.StartConversation(goodEndDialog, null);
        }
    }
}
