using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private Player player;


    public override void DoDamageTo(CharacterStatus targetStatus)
    {
        base.DoDamageTo(targetStatus);
    }

    public override void TakeDamage(float damageValue)
    {
        base.TakeDamage(damageValue);
        player.GetDamagedEffects();
    }

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }


    public override void Die()
    {
        base.Die();
        player.Die();
    }
}
