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
        ConversationManager.OnConversationEnded += ConversationEnded;
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
            loadInventoryIntoConverstation();
            GameManager.Instance.player_busy = true;
        }
    }

    private void loadInventoryIntoConverstation()
    {
        foreach (string item in GameManager.Instance.inventory)
        {
            ConversationManager.Instance.SetBool("Item/" + item, GameManager.Instance.Has_Item_On_Inventory(item));
        }
        
    }

    private void ConversationEnded()
    {
        GameManager.Instance.player_busy = false;
    }

}
