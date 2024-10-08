using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTackBackSwordState : PlayerState
{
    private GameObject swrod;
    private float swordReturnImpact;
    public PlayerTackBackSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        swordReturnImpact = player.swordReturnImpact;
        swrod = player.sword;
        if (player.transform.position.x > player.sword.transform.position.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < player.sword.transform.position.x && player.facingDir == -1)
        {
            player.Flip();
        }

        rb.AddForce(new Vector2(swordReturnImpact * -player.facingDir, 0), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        
        

        if (triggerCalled)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
    }
}
