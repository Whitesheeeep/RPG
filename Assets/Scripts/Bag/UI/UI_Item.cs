using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    [SerializeField] protected Image image;

    public InventoryItem inventoryItem;

    public virtual void CleanUpSlot()
    {
        inventoryItem = null;
        image.sprite = null;
        image.color = Color.clear;
        
    }

    public virtual void UpdateSlots(InventoryItem _newItem)
    {
        inventoryItem = _newItem;
        image.color = Color.white;
        if (inventoryItem != null)
        {
            image.sprite = inventoryItem.itemData.icon;
        }
    }
}
