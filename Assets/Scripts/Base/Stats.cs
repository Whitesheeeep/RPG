using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色的各项数据处理中心，负责处理该项数值在各种情况下的变化（如装备，技能，buff等）
/// </summary>
[System.Serializable]
public class Stats 
{
    [SerializeField] private float baseValue;

    public List<float> modifiers;

    public float GetValue()
    {
        float finalValue = baseValue;
        foreach (var item in modifiers)
        {
            finalValue += item;
        }
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}
