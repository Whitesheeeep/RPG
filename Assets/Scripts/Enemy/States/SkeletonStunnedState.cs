using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.entityFX.InvokeRepeating("RedColorBlink",0, 0.1f);
        stateTimer = enemy.stunnedTime;
        rb.velocity = new Vector2(enemy.stunnedDir.x * -enemy.facingDir, enemy.stunnedDir.y);
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.entityFX.Invoke("CancelColorChange", 0);
    }

}
