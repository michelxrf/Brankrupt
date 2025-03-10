using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Npc : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] GameObject interaction_tip;
    [SerializeField] bool triggerInstantly = false;
    [SerializeField] bool singleUse = false;
    [SerializeField] GameObject conversationsParent;
    
    public int currentConversationIndex;
    public NPCConversation[] conversationList;

    public bool disabled = false;
    private bool playerInRange = false;
    private bool can_interact = false;
    private void Awake()
    {
        can_interact = triggerInstantly;
        if (conversationList.Length < 1)
        {
            Debug.LogError($"{gameObject.name} NPC has no conversation");
            disabled = true;
        }
            

        interaction_tip.SetActive(false);
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }

    private void Start()
    {
        LoadNPCState();
        DisableNPC(disabled);
    }

    private void LoadNPCState()
    {
        if (GameManager.Instance.npcStates.TryGetValue(id, out var npc))
        {
            id = npc.id;
            currentConversationIndex = npc.dialog_index;
            disabled = !npc.active;
            Debug.Log($"{npc.npcName} loaded");
        }
        else
        {
            GameManager.Instance.SaveNPC(id, !disabled, name, currentConversationIndex);
            Debug.Log($"{name} saved");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disabled)
            { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            if (triggerInstantly)
            {
                Interaction();
                can_interact = false;
            }
            else
            {
                interaction_tip.SetActive(true);
                can_interact = true;
            }
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
            playerInRange = false;
            if (triggerInstantly)
            {
                can_interact = false;
            }
            else
            {
                interaction_tip.SetActive(false);
                can_interact = false;
            }
        }
    }

    private void Interaction()
    {
        if (GameManager.Instance.player_busy || GameManager.Instance.is_paused || disabled)
            return;

        if((Input.GetKeyDown(KeyCode.E) && can_interact) || triggerInstantly && playerInRange && can_interact)
        {
            ConversationManager.Instance.StartConversation(conversationList[currentConversationIndex], this);
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

    private void OnConversationEnded()
    {
        GameManager.Instance.player_busy = false;
        if (singleUse && ConversationManager.Instance.conversingNpc == this)
        {
            disabled = true;
            interaction_tip.SetActive(false);
        }
    }

    public void changeConversationIndexTo(int index)
    {
        if (index <= conversationList.Length - 1)
        {
            currentConversationIndex = index;
        }
        else
        {
            currentConversationIndex = 0;
            Debug.LogError("invalid conversation index");
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.SaveNPC(id, !disabled, gameObject.name, currentConversationIndex);
    }

    public void DisableNPC(bool newState)
    {
        disabled = newState;
        gameObject.SetActive(!disabled);
    }

}
