using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float liftPercentage = 0.4f;
    protected override void Start()
    {
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(criticalChance);
        Modify(criticalPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    public void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stats.GetValue() * liftPercentage;

            _stats.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void DoDamageTo(CharacterStatus targetStatus)
    {
        base.DoDamageTo(targetStatus);
    }

    public override void TakeDamage(float damageValue)
    {
        base.TakeDamage(damageValue);
    }
    public override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }





}
