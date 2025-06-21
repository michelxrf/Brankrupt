using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// the game state manager singleton
/// it contains all the main game systems
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }
    [HideInInspector] public hud_manager hud;
    [HideInInspector] public bool is_paused;
    [HideInInspector] public bool player_busy;
    [HideInInspector] public player_controller player;
    [HideInInspector] public bool isGoodEnding;

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
        // called to save an interaction Interaction into memory
        // has different method for Interactions that exists in memorey and those that do not

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
        // used to save into memory a new Interaction

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
    public void UpdateNPC(int id, bool isActive, string npcName, int dialogIndex)
    {
        // used for saving into memory an existing Interaction

        NPCStateHolder updatedNpc = new NPCStateHolder();
        updatedNpc.id = id;
        updatedNpc.npcName = npcName;
        updatedNpc.active = isActive;
        updatedNpc.dialog_index = dialogIndex;

        npcStates[id] = updatedNpc;
    }

    public void PickupFlashlight()
    {
        // used to give the flashlight to the player
        // it's a public method because it need to be acessible to the Dialog Editor

        battery = maxBattery;
        hasFlashlight = true;
        player.ChangeFlashlightAnim();
        hud.PickupFlashlight();
    }


    private void Update()
    {
        VerifyInput();
    }

    private void VerifyInput()
    {
        // verify input that regards whole game logic

        if (Input.GetKeyDown(KeyCode.Escape) && !player_busy)
        {
            TooglePause();
            hud.TooglePauseScreen(is_paused);
        }
    }

    public void ChangeObjective(string objective)
    {
        // used by the Dialog Editor to change the current objective

        gameObjective = objective;
        hud.UpdateObjective();
    }

    private void InitSanity()
    {
        // initalizes the player's sanity according to max value defined by the designer

        currentSanityLevel = maxSanityLevel;
    }

    private void InitFlashlight()
    {
        // intializes the player battery level based on the max level set by the designer

        battery = maxBattery;
    }
    private void InitItemsList()
    {
        // initializes the items in the inventory system
        // it's basically a dict atributing a name to an icon
        // ---
        // DISCLAIMER: this is bad code, but the project had to be rushed
        // given a little more time I'd done this using a better system
        // using Scriptable Objects or an Item Class
        // and a dedicated Class to be the Inventory System
        // instead of jamming everything togheter in the GameManager
        // ---

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

    public void TooglePause()
    {
        // done in a public method to be acessible to the Dialog Editor

        is_paused = !is_paused;
    }

    public void Pause()
    {
        // done in a public method to be acessible to the Dialog Editor

        is_paused = true;
    }

    public void Unpause()
    {
        // done in a public method to be acessible to the Dialog Editor

        is_paused = false;
    }

    public bool Has_Item_On_Inventory(string name)
    {
        // used by the Dialog Editor for checking if the player has certain item
        // this allow for dialog routes to depend on item collection conditions

        return inventory.Contains(name.ToLower());
    }

    public void Add_Item_To_Inventory(string name)
    {
        // to add items to the player inventory, mainly used by the Dialog Editor 

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
        // to remove items to the player inventory, mainly used by the Dialog Editor

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
        // completely empties the player inventory 

        inventory.Clear();
        hud.UpdateInventory();
    }

    public void RechargeBattery(float percentage = 1f)
    {
        // used for refilling the players battery level
        // mainly used by the Dialog Editor

        battery = Mathf.Clamp(battery + maxBattery * percentage, 0, maxBattery);
    }

    public void GameOver()
    {
        // calls the game over screen

        player_busy = true;
        hud.GameOver();
        AudioManager.Instance.GameOver();
    }

    public void BackToMenu()
    {
        // prepares the GameManager for a reset

        Destroy(TransitionManager.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
