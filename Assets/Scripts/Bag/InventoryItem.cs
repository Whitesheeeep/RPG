using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 就是一个装载itemData 的库存物品，同时记录了数量
/// </summary>
[Serializable]
public class InventoryItem 
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
        AddStack();
        
    }
        
    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
