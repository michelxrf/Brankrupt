using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        gameoverScreen.SetActive(false);
        batteryMeter.gameObject.SetActive(false);
    }

    private void Start()
    {
       batteryMeter.gameObject.SetActive(GameManager.Instance.hasFlashlight);

        GameManager.Instance.hud = this;
        batteryMeter.maxValue = GameManager.Instance.maxBattery;
        sanityLevel.maxValue = GameManager.Instance.maxSanityLevel;
        UpdateInventory();
        UpdateObjective();
    }

    public void PickupFlashlight()
    {
        batteryMeter.gameObject.SetActive(true);
    }

    public void UpdateBattery(float value)
    {
        batteryMeter.value = value;
        batteryFiller.color = Color.Lerp(lowBatteryColor, fullBatteryColor, value / GameManager.Instance.maxBattery);
    }

    public void UpdateSanity(float value)
    {
        sanityLevel.value = value;
    }

    public void UpdateInventory()
    {
        ClearInventory();
        FillInventory();
    }

    public void GameOver()
    {
        currentObjectiveObject.SetActive(false);
        itemList.gameObject.SetActive(false);
        sanityLevel.gameObject.SetActive(false);
        batteryMeter.gameObject.SetActive(false);
        gameoverScreen.SetActive(true);
        gameoverSfx.Play();
    }
    public void UpdateObjective()
    {
        currentObjectiveText.text = GameManager.Instance.gameObjective;
    }

    private void FillInventory()
    {
        foreach (var item in GameManager.Instance.inventory)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, itemList.transform);

            newItem.GetComponent<InventoryItemUI>().SetUp(item, GameManager.Instance.allItemsInGame[item]);
        }
        
    }

    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }
    private void ClearInventory()
    {
        foreach (Transform line in itemList.GetComponentsInChildren<Transform>())
        {
            if (line.CompareTag("inventoryItemUI"))
            {
                Destroy(line.gameObject);
            }

        }
    }
}
