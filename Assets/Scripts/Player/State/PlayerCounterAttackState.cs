using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = player.counterAttackWindow;
        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    player.animator.SetBool("SuccessfulCounterAttack", true);
                    stateTimer = 10;
                }
            }
        }

        if(stateTimer < 0 || triggerCalled == true)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

}
