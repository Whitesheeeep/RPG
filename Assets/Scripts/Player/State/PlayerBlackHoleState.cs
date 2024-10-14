using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private bool skillUsed = false;//技能是否使用
    
    private float defaultGravityScale;

    private float flyTimer;
    private float flyTime = .3f;

    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        flyTimer = flyTime;

        //stateTimer = skillDuration;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        if (player.StateMachine.lastState is PlayerGroundState)
        {
            player.SetVelocity(0, player.jumpForce);
        }
        else
        {
            flyTimer = 0f;
            player.SetVelocity(0, 0);
        }
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravityScale;
        skillUsed = false;
        //player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        flyTimer -= Time.deltaTime;
        if (flyTimer <= 0 )
        {
            player.SetVelocity(0, -0.1f);
            if (!skillUsed)
            {
                if (player.skill.blackHoleSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        
    }
}
