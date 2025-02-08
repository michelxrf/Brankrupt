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
    [SerializeField] public Dictionary<int, NPCStateHolder> npcStates = new Dictionary<int, NPCStateHolder>();

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

    public void SaveNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        if (npcStates.ContainsKey(id))
        {
            UpdateNPC(id, isActive, npcName, dialogIndex);
        }
        else
        {
            RegisterNPC(id, isActive, npcName, dialogIndex);
        }
    }
    public void RegisterNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        NPCStateHolder newNpc = new NPCStateHolder();
        newNpc.id = id;
        newNpc.npcName = npcName;
        newNpc.active = isActive;
        newNpc.dialog_index = dialogIndex;

        if (newNpc.AssertCorrectConfig())
        {
            npcStates.Add(id, newNpc);
        }
            
    }

    private void PrintAllNpc()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            foreach (var npc in npcStates)
            {
                Debug.Log($"{npc.Key} - {npc.Value.npcName}");
            }
            
    }

    public void SkipToNextDialog(int npcId)
    {
        if (GameManager.Instance.npcStates.TryGetValue(npcId, out var npc))
        {
            UpdateNPC(npcId, npc.active, npc.npcName, npc.dialog_index + 1);
        }
        else
        {
            Debug.LogError("Npc index not found when skipping dialog");
        }

        
    }

    public void UpdateNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        NPCStateHolder updatedNpc = new NPCStateHolder();
        updatedNpc.id = id;
        updatedNpc.npcName = npcName;
        updatedNpc.active = isActive;
        updatedNpc.dialog_index = dialogIndex;

        npcStates[id] = updatedNpc;
        Debug.Log($"{npcStates[id].npcName} Updated");
    }

    private void Update()
    {
        DebugBackToMenu();
        PrintAllNpc();
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
        allItemsInGame.Add("agenda", "InventoryIcons/genericItem_color_155");
        allItemsInGame.Add("documentos", "InventoryIcons/genericItem_color_155");
        allItemsInGame.Add("documents", "InventoryIcons/genericItem_color_148");
        allItemsInGame.Add("screwdriver", "InventoryIcons/genericItem_color_005");
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
        if (!allItemsInGame.ContainsKey(name.ToLower().Trim()))
        {
            Debug.LogError("Trying to add an invalid item to the inventory");
            return;
        }

        inventory.Add(name.ToLower().Trim());
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

    public void DeactivateNPC(int id)
    {
        UpdateNPC(id, false, npcStates[id].npcName, npcStates[id].dialog_index);
    }

    public void ActivateNPC(int id)
    {
        UpdateNPC(id, true, npcStates[id].npcName, npcStates[id].dialog_index);
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
