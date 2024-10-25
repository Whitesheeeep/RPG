using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTirggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStatus playerStatus = hit.GetComponent<PlayerStatus>();
                enemy.GetComponent<EnemyStatus>().DoDamageTo(playerStatus);
            }
        }
    }

    protected void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    protected void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
