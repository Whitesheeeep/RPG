using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    private Inventory inventory => Inventory.instance;
    private List<InventoryItem> playerEquippedItems;


    public override void GenerateDrop()
    {
        playerEquippedItems = inventory.GetPlayerEquipmentItems();
        Debug.Log("Éú³ÉµôÂä");
        inventory.SetReturnToInventory(false);
        int forLoopCount = playerEquippedItems.Count;
        for (int i = 0; i < forLoopCount; i++)
        {
            DropItem(playerEquippedItems[0].itemData);
            inventory.PlayerUnEquipWith(playerEquippedItems[0].itemData as Equipment_ItemData);
        }
        
        inventory.SetReturnToInventory(true);
    }
}
