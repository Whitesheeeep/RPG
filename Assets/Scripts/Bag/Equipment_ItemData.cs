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

    [Header("Major stats")]
    public int strength; // ���� ����1�� �������� 1% �￹
    public int agility;// ���� ���� 1% ���ܼ������� 1%
    public int intelligence;// 1 �� ħ���˺� 1��ħ�� 
    public int vitality;//��Ѫ��

    [Header("Offensive stats")]
    public int damage;
    public int criticalChance;      // ������
    public int criticalPower;       //150% ����

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;//����ֵ
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

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
}
