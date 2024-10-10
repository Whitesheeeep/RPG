using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{

    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();

        //优化手感，以免刚扔出去剑就滑步出去
        player.StartCoroutine("BusyFor",0.2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if (Input.GetKeyUp(KeyCode.F))
        {
            stateMachine.ChangeState(player.IdleState);
            
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > mousePosition.x && player.facingDir == 1)
        {
            player.Flip();
        }
        else if(player.transform.position.x < mousePosition.x && player.facingDir == -1)
        {
            player.Flip();
        }

    }
}
