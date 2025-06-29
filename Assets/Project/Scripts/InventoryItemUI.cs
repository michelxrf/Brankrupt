using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// defines an inventory item
/// will be part of player's inventory
/// and shown on the inventory UI
/// </summary>
public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] public Image icon;
    public void SetUp(string name, string iconPath)
    {
        itemName.text = name;
        
        Sprite newImage = Resources.Load<Sprite>(iconPath);
        icon.sprite = newImage;
        icon.overrideSprite = newImage;
    }
}
