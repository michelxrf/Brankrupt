using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// used to connect the HUD visual elements with game systems
/// like player sanity, inventory system, game over and pause menus
/// </summary>
public class hud_manager : MonoBehaviour
{  
    [Header("Flashlight")]
    [SerializeField] Slider batteryMeter;
    [SerializeField] Image batteryFiller;
    [SerializeField] Color fullBatteryColor;
    [SerializeField] Color lowBatteryColor;

    [Header("Sanity")]
    [SerializeField] Slider sanityLevel;

    [Header("Inventory")]
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GameObject itemList;

    [Header("Objective Marker")]
    [SerializeField] GameObject currentObjectiveObject;
    [SerializeField] TextMeshProUGUI currentObjectiveText;

    [Header("Game Over")]
    [SerializeField] AudioSource gameoverSfx;
    [SerializeField] GameObject gameoverScreen;

    [Header("Quit")]
    [SerializeField] GameObject quitPanel;

    private void Awake()
    {
        // hides the informationtat needs to start hidden

        gameoverScreen.SetActive(false);
        batteryMeter.gameObject.SetActive(false);
        quitPanel.SetActive(false);
    }

    private void Start()
    {
        // initialize sliders data and inventory

        batteryMeter.gameObject.SetActive(GameManager.Instance.hasFlashlight);

        GameManager.Instance.hud = this;
        batteryMeter.maxValue = GameManager.Instance.maxBattery;
        sanityLevel.maxValue = GameManager.Instance.maxSanityLevel;
        UpdateInventory();
        UpdateObjective();

        if (GameManager.Instance.hasFlashlight)
        {
            PickupFlashlight();
        }

    }

    public void Unpause()
    {
        // makes the game unpause

        GameManager.Instance.Unpause();
        TooglePauseScreen(false);
    }

    public void TooglePauseScreen(bool state)
    {
        // toggles pause menu visibility

        quitPanel.SetActive(state);
    }

    public void PickupFlashlight()
    {
        // turns on flashlight related UI elements

        batteryMeter.gameObject.SetActive(true);
        UpdateBattery(GameManager.Instance.battery);
    }

    public void UpdateBattery(float value)
    {
        // updates flashlight battery on its UI element

        batteryMeter.value = value;
        batteryFiller.color = Color.Lerp(lowBatteryColor, fullBatteryColor, value / GameManager.Instance.maxBattery);
    }

    public void UpdateSanity(float value)
    {
        // updates sanity level to its respective UI element

        sanityLevel.value = value;
    }

    public void UpdateInventory()
    {
        // refreshes the inventory

        ClearInventory();
        FillInventory();
    }

    public void GameOver()
    {
        // disables every UI element and shows the gameover screen

        currentObjectiveObject.SetActive(false);
        itemList.gameObject.SetActive(false);
        sanityLevel.gameObject.SetActive(false);
        batteryMeter.gameObject.SetActive(false);
        gameoverScreen.SetActive(true);
        gameoverSfx.Play();
    }
    public void UpdateObjective()
    {
        // updates the player's current objective to the UI

        currentObjectiveText.text = GameManager.Instance.gameObjective;
    }

    private void FillInventory()
    {
        // instantiate the inventory items to the inventory UI

        foreach (var item in GameManager.Instance.inventory)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, itemList.transform);

            newItem.GetComponent<InventoryItemUI>().SetUp(item, GameManager.Instance.allItemsInGame[item]);
        }
        
    }

    public void BackToMenu()
    {
        // used by UI button to call a return to the main menu

        GameManager.Instance.BackToMenu();
    }
    private void ClearInventory()
    {
        // clears the inventory UI from all item elements

        foreach (Transform line in itemList.GetComponentsInChildren<Transform>())
        {
            if (line.CompareTag("inventoryItemUI"))
            {
                Destroy(line.gameObject);
            }

        }
    }
}
