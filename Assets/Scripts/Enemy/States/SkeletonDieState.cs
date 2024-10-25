using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkeletonDieState : EnemyState
{
    private Enemy_Skeleton enemy;
    private SpriteRenderer sr;
    private Animator animator => enemy.animator;

    public float fadeSpeed = 0.5f;
    public SkeletonDieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        sr = enemy.GetComponentInChildren<SpriteRenderer>();
        fadeSpeed = enemy.fadeSpeed;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.speed = 0;
        enemy.moveSpeed = 0;

        sr.color = new Color(1, 1, 1, 0.5f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(sr.color != Color.clear)
        {
            sr.color = Color.Lerp(sr.color, Color.clear, fadeSpeed * Time.deltaTime);
        }
        else
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
