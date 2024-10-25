using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    private Enemy enemy;

    public override void Die()
    {
        base.Die();
        enemy.Die();
    }

    public override void DoDamageTo(CharacterStatus targetStatus)
    {
        base.DoDamageTo(targetStatus);
    }

    public override void TakeDamage(float damageValue)
    {
        base.TakeDamage(damageValue);
        enemy.GetDamagedEffects();
    }

    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
    }




}
