using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    [SerializeField] private TextMeshProUGUI itemText;
    protected override void Start()
    {
        base.Start();
        
    }

    public void SetUpCraftSlot(Equipment_ItemData _data)
    {
        if (_data == null) return;

        inventoryItem.itemData = _data;
        image.sprite = _data.icon;
        itemText.text = _data.name;

        if (_data.name.Length > 16) itemText.fontSize *= .7f;
    }

    private void OnValidate()
    {
        if (inventoryItem.itemData != null)
            gameObject.name = inventoryItem.itemData.itemName + " Slot";
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        Equipment_ItemData targetItem = inventoryItem.itemData as Equipment_ItemData;

        uI_Menu.craftDescpWindow.SetUpCraftWindow(targetItem);
    }

    

    
}
