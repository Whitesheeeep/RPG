using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_Item, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Equipment_ItemData targetItem = inventoryItem.itemData as Equipment_ItemData;

        Inventory.instance.CanCraft(targetItem, targetItem.craftRequirementMaterials);
    }

    private void OnEnable()
    {
        UpdateSlots(inventoryItem);
    }

    
}
