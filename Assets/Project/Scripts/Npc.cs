using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] GameObject interaction_tip;
    [SerializeField] NPCConversation conversation;
    
    private bool can_interact = false;
    private void Awake()
    {
        interaction_tip.SetActive(false);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interaction_tip.SetActive(true);
            can_interact=true;
        }
    }

    private void Update()
    {
        Interaction();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interaction_tip.SetActive(false);
            can_interact=false;
        }
    }

    private void Interaction()
    {
        if (GameManager.Instance.player_busy || GameManager.Instance.is_paused)
            return;

        if(Input.GetKeyDown(KeyCode.E) && can_interact)
        {
            ConversationManager.Instance.StartConversation(conversation);
            GameManager.Instance.player_busy = true;
        }
    }

}
