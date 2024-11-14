using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private Player player;
    private PlayerItemDrop playerDropSystem;


    public override void DoDamageTo(CharacterStatus targetStatus)
    {
        base.DoDamageTo(targetStatus);
    }

    public override void TakeDamage(float damageValue)
    {
        base.TakeDamage(damageValue);
    }

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        playerDropSystem = GetComponent<PlayerItemDrop>();
    }

    protected override void DecreaseHealthBy(float _damage)
    {
        base.DecreaseHealthBy(_damage);

        Equipment_ItemData equipment_ItemData = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (equipment_ItemData != null)
        {
            equipment_ItemData.ExecuteItemEffect(null);
        }
    }

    public override void Die()
    {
        base.Die();
        player.Die();

        playerDropSystem.GenerateDrop();
    }
}
