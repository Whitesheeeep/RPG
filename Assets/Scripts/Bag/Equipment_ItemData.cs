using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
public class Equipment_ItemData : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;
    public List<ItemEffects> itemEffects;

    #region 武器的各项属性

    [Header("Major stats")]
    public int strength; // 力量 增伤1点 爆伤增加 1% 物抗

    public int agility;// 敏捷 闪避 1% 闪避几率增加 1%
    public int intelligence;// 1 点 魔法伤害 1点魔抗
    public int vitality;//加血的

    [Header("Offensive stats")]
    public int damage;

    public int criticalChance;      // 暴击率
    public int criticalPower;       //150% 爆伤

    [Header("Defensive stats")]
    public int maxHealth;

    public int armor;
    public int evasion;//闪避值
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;

    public int iceDamage;
    public int lightingDamage;
    #endregion 武器的各项属性

    private int descrptionLength = 0;//用于控制最小的文本面积

    private void OnValidate()
    {
        itemName = name;
    }

    public void AddModifiers()
    {
        PlayerStatus playerStats = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.criticalChance.AddModifier(criticalChance);
        playerStats.criticalPower.AddModifier(criticalPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStatus playerStats = PlayerManager.instance.player.GetComponent<PlayerStatus>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.criticalChance.RemoveModifier(criticalChance);
        playerStats.criticalPower.RemoveModifier(criticalPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public void ExecuteItemEffect(Transform targetTransform)
    {
        foreach (var item in itemEffects)
        {
            item?.ExecuteEffect(targetTransform);
        }
    }

    public override string GetDescription()
    {
        itemDescription_Sb.Length = 0;
        descrptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(criticalChance, "Critical Chance");
        AddItemDescription(criticalPower, "Critical Power");

        AddItemDescription(maxHealth, "Max Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightingDamage, "Lighting Damage");

        if (descrptionLength < 5)
        {
            for (int i = 0; i < 5 - descrptionLength; i++)
            {
                itemDescription_Sb.AppendLine();
                itemDescription_Sb.Append("");
            }
        }

        return itemDescription_Sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            if (itemDescription_Sb.Length >= 0)
            {
                itemDescription_Sb.AppendLine();
            }
            if(_value > 0)
            {
                itemDescription_Sb.Append("+" + _value + "\t" + _name);
            }
            descrptionLength++;
        }

    }
}