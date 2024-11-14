using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : UI_Item, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private TextMeshProUGUI itemCount;
    protected UI_Menu uI_Menu;

    protected virtual void Start()
    {
        uI_Menu = GetComponentInParent<UI_Menu>();
    }

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
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //将物品丢弃 
                if (inventoryItem.itemData.itemType == ItemType.Material)
                {
                    Inventory.instance.RemoveItemFromStash(inventoryItem.itemData);
                    return;
                }
                Inventory.instance.RemoveItemFromInventory(inventoryItem.itemData);

                return;
            }


            if (inventoryItem.itemData.itemType == ItemType.Equipment)
            {
                //将装备放在角色的装备中
                Inventory.instance.PlayerEquipWith(inventoryItem.itemData as Equipment_ItemData);
            }
        }

        uI_Menu.descriptionToolTip.HideToolTip();
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
