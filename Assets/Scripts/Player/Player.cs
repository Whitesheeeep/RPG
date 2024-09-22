using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    
    
    [Header("Attack Info")]
    public Vector2[] attackMotion;

    [Header("Move Info")]
    public float moveSpeed = 8f;
    public float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCoolDown;
    private float dashCoolTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public float dashSmooth;

    public float wallJumpDuration;




    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    #endregion States

    protected override void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(StateMachine, this, "Idle");
        MoveState = new PlayerMoveState(StateMachine, this, "Move");
        JumpState = new PlayerJumpState(StateMachine, this, "Jump");
        InAirState = new PlayerInAirState(StateMachine, this, "Jump");
        DashState = new PlayerDashState(StateMachine, this, "Dash");
        WallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");
        WallJumpState = new PlayerWallJumpState(StateMachine, this, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(StateMachine, this, "Attack_1");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
        
    }
        
    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();
        //Debug.Log(StateMachine.currentState);
    }



    public void CheckForDashInput()
    {
        dashCoolTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolTimer < 0)
        {
            dashCoolTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");  
            if(dashDir == 0)
            {
                dashDir = facingDir;
            }
            StateMachine.ChangeState(DashState);
        }
    }



    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();
}
