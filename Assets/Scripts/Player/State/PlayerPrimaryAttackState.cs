using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int attackCounter { get; private set; } = 0;

    private float lastAttackTime;
    private float comboWindow = 2f;
    public PlayerPrimaryAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = Input.GetAxisRaw("Horizontal");//修复攻击有时与方向不一致的的bug，问题可能出现在xInput 仍然保留的是上一个 Update 中的数据。
        //xInput 输入之前已经改变了
        float attackDir = (xInput == 0) ? player.facingDir : xInput;

        if (attackCounter > 2 || Time.time - lastAttackTime > comboWindow)
        {
            attackCounter = 0;
        }

        

        player.SetVelocity(player.attackMotion[attackCounter].x * attackDir, player.attackMotion[attackCounter].y);
        stateTimer = 0.1f;
        player.animator.SetInteger("attackCounter", attackCounter);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.12f);
        attackCounter++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            player.SetVelocity(0, 0);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
