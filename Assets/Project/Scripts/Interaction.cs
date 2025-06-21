using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// controls all the level's interactions
/// like NPCs and objects
/// Interactions states are held in memory
/// so they keep they states as the player move between rooms
/// and scene changes
/// </summary>
public class Interaction : MonoBehaviour
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
        // loads the interaction state as the object is loaded in game

        if (GameManager.Instance.npcStates.TryGetValue(id, out var npc))
        {
            id = npc.id;
            currentConversationIndex = npc.dialog_index;
            disabled = !npc.active;
            Debug.Log($"{name} loaded");
        }
        else
        {
            // if the object isn't found in memory it means this is the first time it appears
            // then its saved to memory
            GameManager.Instance.SaveNPC(id, !disabled, name, currentConversationIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // allows player to interact with it

        if (disabled)
            { return; }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

            // instant trigger if desired by level designer
            if (triggerInstantly)
            {
                Interact();
            }

            // or wait for playe input
            else
            {
                interaction_tip.SetActive(true);
                can_interact = true;
            }
        }
    }

    private void Update()
    {
        Interact();
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

    private void Interact()
    {
        // calls the actual interaction from the dialog system

        if (GameManager.Instance.player_busy || GameManager.Instance.is_paused || disabled || !playerInRange)
            return;

        if((Input.GetKeyDown(KeyCode.E) && can_interact) || (triggerInstantly && playerInRange && can_interact))
        {
            can_interact = false;
            ConversationManager.Instance.StartConversation(conversationList[currentConversationIndex], this);
            loadInventoryIntoConverstation();
            GameManager.Instance.player_busy = true;
        }
    }

    private void loadInventoryIntoConverstation()
    {
        // loads the current player inventory into the dialog system
        // this allows the dialog system to test if player has certain
        // items in the inventory and act accordinly

        foreach (string item in GameManager.Instance.inventory)
        {
            ConversationManager.Instance.SetBool("Item/" + item, GameManager.Instance.Has_Item_On_Inventory(item));
        }
        
    }

    private void OnConversationEnded()
    {
        // deals with the end of interaction
        // some interactions are single use
        // while others can be interacted again

        GameManager.Instance.player_busy = false;
        if (singleUse && ConversationManager.Instance.conversingNpc == this)
        {
            disabled = true;
            interaction_tip.SetActive(false);
        }
        else if(playerInRange && !disabled)
        {
            can_interact = true;
        }
    }

    public void changeConversationIndexTo(int index)
    {
        // allow other objects to change this intercation current conversation
        // meaning one interactable object can modify others through the dialog system

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
        // saves its state to memory

        GameManager.Instance.SaveNPC(id, !disabled, gameObject.name, currentConversationIndex);
    }

    public void DisableNPC(bool newState)
    {
        // alllows for other objects to disable this through the dialog system

        disabled = newState;
        gameObject.SetActive(!disabled);
    }

}
