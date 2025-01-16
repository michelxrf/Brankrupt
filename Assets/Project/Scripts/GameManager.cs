using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool is_paused;
    public bool player_busy;
    public static GameManager Instance { get; private set; }
    public List<string> inventory = new List<string>();

    public hud_manager hud;

    [Header("Battery")]
    public float batteryDrain = 1f;
    public float flashlightRange = 4f;
    public float battery;
    public float maxBattery = 100f;

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

        InitFlashlight();

    }

    private void Update()
    {
        DebugBackToMenu();
    }

    private void InitFlashlight()
    {
        battery = maxBattery;
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

    public void RechargeBattery(float percentage = 1f)
    {
        battery = Mathf.Clamp(battery + maxBattery * percentage, 0, maxBattery);
    }

    private void DebugBackToMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
