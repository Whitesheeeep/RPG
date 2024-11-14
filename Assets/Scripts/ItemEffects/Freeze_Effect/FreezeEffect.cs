using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Freeze Effect", menuName = "Data/Item Effects/Freeze Effect")]
public class FreezeEffect : ItemEffects
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform targetTransform)
    {
        if (!Inventory.instance.CanUseArmor()) return;

        Player player = PlayerManager.instance.player;
        Collider2D[] targetsEnemy = Physics2D.OverlapCircleAll(player.transform.position, 4f);
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        if (playerStatus.currentHealth > playerStatus.GetMaxHealth() * 0.3f) return;



        foreach (Collider2D target in targetsEnemy)
        {
            if (target.GetComponent<Enemy>() != null)
            {
                target.GetComponent<Enemy>().FreezeTimeFor(freezeDuration);
            }
        }
    }
}
