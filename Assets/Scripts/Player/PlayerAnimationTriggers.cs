using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
        //player.StateMachine.ChangeState(player.IdleState);
    }

    private void AttackTrigger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.GetComponent<Enemy>() != null) 
            {
                EnemyStatus enemyStatus = hit.GetComponent<EnemyStatus>();
                player.GetComponent<PlayerStatus>().DoDamageTo(enemyStatus);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.swordSkill.UseSkill();
    }
}
