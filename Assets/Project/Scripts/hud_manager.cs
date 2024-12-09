using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hud_manager : MonoBehaviour
{
    [SerializeField] Slider battery_level;
    [SerializeField] TextMeshProUGUI itemsLabel;

    private void Awake()
    {
        battery_level.gameObject.SetActive(false);
        GameManager.Instance.hud = this;
        Update_Inventory();
    }

    public void Update_Battery(float value)
    {
        battery_level.value = value;
    }

    public void Update_Inventory()
    {
        itemsLabel.text = "Inventory: \n";

        foreach (string item in GameManager.Instance.inventory)
        {
            itemsLabel.text += item + "\n";
        }
    }

    public void ShowBatteryLevel()
    {
        battery_level.gameObject.SetActive(true);
    }
}
