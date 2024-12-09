using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool is_paused;
    public bool player_busy;
    public static GameManager Instance { get; private set; }
    public List<string> inventory = new List<string>();
    public Dictionary<Guid, int> npc_current_index_dict = new Dictionary<Guid, int>();

    public hud_manager hud;

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

    public void Pause()
    {
        is_paused = true;
    }

    public void Unpause()
    {
        is_paused = false;
    }

    public bool Has_Item_On_Inventory(string name)
    {
        return inventory.Contains(name.ToLower());
    }

    public void Add_Item_To_Inventory(string name)
    {
        inventory.Add(name.ToLower());
        hud.Update_Inventory();
    }

    public void Remove_Item_From_Inventory(string name)
    {
        inventory.Remove(name.ToLower());
        hud.Update_Inventory();
    }

    public void Clear_Inventory()
    {
        inventory.Clear();
        hud.Update_Inventory();
    }

    public void Log_All_Items()
    {
        foreach (var item in inventory)
        {
            Debug.Log(item);
        }
    }
}
