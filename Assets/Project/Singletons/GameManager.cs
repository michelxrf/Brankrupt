using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool is_paused;
    public bool player_busy;
    public static GameManager Instance { get; private set; } 
    private List<string> inventory = new List<string>();

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
        return inventory.Contains(name);
    }

    public void Add_Item_To_Inventory(string name)
    {
        inventory.Add(name);
    }

    public void Remove_Item_From_Inventory(string name)
    {
        inventory.Remove(name);
    }

    public void Clear_Inventory()
    {
        inventory.Clear();
    }

    public void Log_All_Items()
    {
        foreach (var item in inventory)
        {
            Debug.Log(item);
        }
    }
}
