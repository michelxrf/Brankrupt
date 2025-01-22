using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }

    [HideInInspector] public hud_manager hud;
    [HideInInspector] public bool is_paused;
    [HideInInspector] public bool player_busy;

    [Header("Inventory")]
    [SerializeField] public List<string> inventory = new List<string>();
    private Dictionary<string, string> allItemsInGame = new Dictionary<string, string>();

    [Header("Flashlight Settings")]
    [SerializeField] public float batteryDrain = 1f;
    [SerializeField] public float flashlightRange = 4f;
    [SerializeField] public float battery;
    [SerializeField] public float maxBattery = 100f;

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

    private void InitItemsList()
    {

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
        if (!allItemsInGame.ContainsKey(name))
        {
            Debug.LogError("Trying to add an invalid item to the inventory");
        }

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
