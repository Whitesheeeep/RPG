using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType 
{
    Weapon, 
    Armor, 
    Accessory,
    Flask
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment_ItemData : ItemData
{
    public EquipmentType equipmentType;
}
