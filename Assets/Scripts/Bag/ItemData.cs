using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    protected StringBuilder itemDescription_Sb = new StringBuilder();

    [Range(0,100)]
    public float dropChance;

    public virtual string GetDescription()
    {

        return "";
    }
}
