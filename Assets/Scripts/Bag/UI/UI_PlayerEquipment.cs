using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerEquipment : UI_Item, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EquipmentType EquipmentType;
    public UI_Menu uI_Menu;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot-"+EquipmentType.ToString();
    }

    private void Start()
    {
        uI_Menu = GetComponentInParent<UI_Menu>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem != null)
        {
            Inventory.instance.PlayerUnEquipWith(inventoryItem.itemData as Equipment_ItemData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryItem != null && inventoryItem.itemData != null)
        {
            uI_Menu.descriptionToolTip.ShowToolTip(inventoryItem.itemData as Equipment_ItemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryItem != null && inventoryItem.itemData != null)
        {
            uI_Menu.descriptionToolTip.HideToolTip();
        }
    }
}
