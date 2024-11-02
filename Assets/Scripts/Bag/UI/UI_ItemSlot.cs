using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : UI_Item, IPointerDownHandler
{
    
    [SerializeField] private TextMeshProUGUI itemCount;

    public override void UpdateSlots(InventoryItem _newItem)
    {
        inventoryItem = _newItem;
        image.color = Color.white;
        if (inventoryItem != null)
        {
            image.sprite = inventoryItem.itemData.icon;
            if (inventoryItem.stackSize >= 1)
            {
                itemCount.text = inventoryItem.stackSize.ToString();
            }
            else
            {
                itemCount.text = "";
            }
        }
    }

    public override void CleanUpSlot()
    {
        base.CleanUpSlot();
        itemCount.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
        if (inventoryItem != null && inventoryItem.itemData != null)
        {
            if (inventoryItem.itemData.itemType == ItemType.Equipment)
            {
                //将装备放在角色的装备中
                Inventory.instance.PlayerEquipWith(inventoryItem.itemData as Equipment_ItemData);
            }
        }
        
    }
}
