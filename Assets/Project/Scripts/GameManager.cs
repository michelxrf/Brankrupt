using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogueEditor;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }

    [HideInInspector] public hud_manager hud;
    [HideInInspector] public bool is_paused;
    [HideInInspector] public bool player_busy;

    [Header("Inventory")]
    [SerializeField] public List<string> inventory = new List<string>();
    [SerializeField] public Dictionary<string, string> allItemsInGame = new Dictionary<string, string>();

    [Header("Flashlight Settings")]
    [SerializeField] public float batteryDrain = 1f;
    [SerializeField] public float flashlightRange = 4f;
    [SerializeField] public float battery;
    [SerializeField] public float maxBattery = 100f;

    [Header("Guide")]
    [SerializeField] public string gameObjective = "What should I do?";

    [Header("NPC States")]
    [SerializeField] public List<NPCStateHolder> npcStates = new List<NPCStateHolder>();

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
            InitFlashlight();
            InitItemsList();
        }
        
    }

    private void Start()
    {

    }

    public void SaveNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        NPCStateHolder npc = npcStates.Find(npc => npc.id == id);

        if (npc == null)
        {
            RegisterNPC(id, isActive, npcName, dialogIndex);
        }
        else
        {
            UpdateNPC(id, isActive, npcName, dialogIndex);
        }
    }

    public NPCStateHolder LoadNPC(int id)
    {
        NPCStateHolder npc = npcStates.Find(npc => npc.id == id);

        if (npc == null)
        {
            return null;
        }
        else
        {
            return npc;
        }
    }

    public void RegisterNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        if (npcStates.Find(npc => npc.id == id))
        {
            Debug.LogError("trying to register an NPC with an id that's already registered");
            return;
        }

        NPCStateHolder newNpc = new NPCStateHolder();
        newNpc.id = id;
        newNpc.npcName = npcName;
        newNpc.active = isActive;
        newNpc.dialog_index = dialogIndex;

        if (newNpc.AssertCorrectConfig())
            npcStates.Add(newNpc);
    }

    public void UpdateNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        NPCStateHolder updatedNpc = npcStates.Find(npc => npc.id == id); ;
        updatedNpc.npcName = npcName;
        updatedNpc.active = isActive;
        updatedNpc.dialog_index = dialogIndex;
    }

    private void Update()
    {
        DebugBackToMenu();
    }

    public void ChangeObjective(string objective)
    {
        gameObjective = objective;
        hud.UpdateObjective();
    }

    private void InitFlashlight()
    {
        battery = maxBattery;
    }
    private void InitItemsList()
    {
        allItemsInGame.Add("pendrive", "InventoryIcons/genericItem_color_155");
        allItemsInGame.Add("key", "InventoryIcons/genericItem_color_155");
        allItemsInGame.Add("documents", "InventoryIcons/genericItem_color_148");
        allItemsInGame.Add("screwdriver", "InventoryIcons/genericItem_color_005");
        allItemsInGame.Add("Caneta", "InventoryIcons/genericItem_color_005");// add art
        allItemsInGame.Add("Contrato", "InventoryIcons/genericItem_color_005");// add art
        allItemsInGame.Add("Carimbo", "InventoryIcons/genericItem_color_005");// add art
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
            return;
        }

        inventory.Add(name.ToLower());
        hud.UpdateInventory();
    }

    public void Remove_Item_From_Inventory(string name)
    {
        if (Has_Item_On_Inventory(name))
        {
            inventory.Remove(name.ToLower());
            hud.UpdateInventory();
        }
        else
        {
            Debug.Log("trying to remove an item that's not present on the inventory");
        }
    }

    public void Clear_Inventory()
    {
        inventory.Clear();
        hud.UpdateInventory();
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
