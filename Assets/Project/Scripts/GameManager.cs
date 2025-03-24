using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogueEditor;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }
    [HideInInspector] public hud_manager hud;
    [HideInInspector] public bool is_paused;
    [HideInInspector] public bool player_busy;
    [HideInInspector] public player_controller player;

    [Header("Inventory")]
    [SerializeField] public List<string> inventory = new List<string>();
    [SerializeField] public Dictionary<string, string> allItemsInGame = new Dictionary<string, string>();

    [Header("Flashlight Settings")]
    [SerializeField] public float batteryDrain = 1f;
    [SerializeField] public float flashlightRange = 4f;
    [SerializeField] public float battery;
    [SerializeField] public float maxBattery = 100f;
    [HideInInspector] public bool flashlightOn = false;
    [SerializeField] public bool hasFlashlight = false;

    [Header("Sanity")]
    [SerializeField] public float maxSanityLevel = 100f;
    [SerializeField] public float sanityDrain = 10f;
    [SerializeField] public float sanityMultiplier = 1f;
    [SerializeField] public float currentSanityLevel;
    [SerializeField] public float globalLightTreshold = .25f;
    [SerializeField] public float currentGlobalLight = 0f;
    [SerializeField] public float sanityRecoveryFactor = 1.5f;
    [SerializeField] public bool closeToMonster = false;

    [Header("Guide")]
    [SerializeField] public string gameObjective = "What should I do?";

    [Header("State Holders")]
    [SerializeField] public Dictionary<int, NPCStateHolder> npcStates = new Dictionary<int, NPCStateHolder>();
    [SerializeField] public Dictionary<int, bool> transitionStates = new Dictionary<int, bool>();

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            InitFlashlight();
            InitItemsList();
            InitSanity();
        }

    }

    public void SaveNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        if (npcStates.ContainsKey(id))
        {
            UpdateNPC(id, isActive, npcName, dialogIndex);
            Debug.Log($"{npcName} updated, index {dialogIndex}");
        }
        else
        {
            RegisterNPC(id, isActive, npcName, dialogIndex);
            Debug.Log($"{npcName} registered, index {dialogIndex}");
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

    public void PickupFlashlight()
    {
        battery = maxBattery;
        hasFlashlight = true;
        player.ChangeFlashlightAnim();
        hud.PickupFlashlight();
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
    }

    public void ChangeObjective(string objective)
    {
        gameObjective = objective;
        hud.UpdateObjective();
    }

    private void InitSanity()
    {
        currentSanityLevel = maxSanityLevel;
    }

    private void InitFlashlight()
    {
        battery = maxBattery;
    }
    private void InitItemsList()
    {
        allItemsInGame.Add("agenda", "InventoryIcons/agenda");
        allItemsInGame.Add("documentos", "InventoryIcons/documento");
        allItemsInGame.Add("chave", "InventoryIcons/chave_vermelha");
        allItemsInGame.Add("lanterna", "InventoryIcons/lanterna");
        allItemsInGame.Add("calças", "InventoryIcons/Calca");
        allItemsInGame.Add("chapéu", "InventoryIcons/Chapeu");
        allItemsInGame.Add("sapatos", "InventoryIcons/Botas");
        allItemsInGame.Add("casaco", "InventoryIcons/Casaco");
        allItemsInGame.Add("cristal", "InventoryIcons/Cristal");
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
        
        if (name == "lanterna")
        {
            PickupFlashlight();
        }
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
            BackToMenu();
        }
    }

    public void GameOver()
    {
        player_busy = true;
        // TODO: play Death Anim
        hud.GameOver();
        AudioManager.Instance.GameOver();
    }

    public void BackToMenu()
    {
        Destroy(TransitionManager.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
