using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

/// <summary>
/// 用于库存物品的物品数据
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public ItemType itemType;

    [Range(0,100)]
    public float dropChance;
}
