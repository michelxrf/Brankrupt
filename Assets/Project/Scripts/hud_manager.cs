using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hud_manager : MonoBehaviour
{
    [SerializeField] Slider battery_level;
    [SerializeField] Slider sanityLevel;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GameObject itemList;
    [SerializeField] TextMeshProUGUI currentObjective;

    private void Awake()
    {
    }

    private void Start()
    {
        GameManager.Instance.hud = this;
        battery_level.maxValue = GameManager.Instance.maxBattery;
        sanityLevel.maxValue = GameManager.Instance.maxSanityLevel;
        UpdateInventory();
        UpdateObjective();
    }

    public void UpdateBattery(float value)
    {
        battery_level.value = value;
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

    public void UpdateObjective()
    {
        currentObjective.text = GameManager.Instance.gameObjective;
    }

    private void FillInventory()
    {
        foreach (var item in GameManager.Instance.inventory)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, itemList.transform);

            newItem.GetComponent<InventoryItemUI>().SetUp(item, GameManager.Instance.allItemsInGame[item]);
        }
        
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
