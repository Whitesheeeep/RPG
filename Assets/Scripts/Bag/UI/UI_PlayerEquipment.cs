using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerEquipment : UI_Item, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            Inventory.instance.PlayerUnEquipWith(inventoryItem.itemData as Equipment_ItemData);
        }
    }
}
