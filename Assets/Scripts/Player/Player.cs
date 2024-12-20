using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    public GameObject sword { get; private set; }

    [Header("Attack Info")]
    public Vector2[] attackMotion;

    public float counterAttackWindow;

    [Header("Move Info")]
    public float moveSpeed = 8f;

    public float jumpForce;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;

    private float defaultDashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    public float dashSmooth;

    public float wallJumpDuration;

    [HideInInspector] public SkillManager skill;

    public float swordReturnImpact;

    //BlackHole �ܷ�������
    [HideInInspector] public bool canBlackHoleReleased = true;

    #region States

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerTackBackSwordState TackBackSwordState { get; private set; }
    public PlayerBlackHoleState BlackHoleState { get; private set; }
    public PlayerDieState dieState { get; private set; }

    #endregion States

    protected override void Awake()
    {
        skill = SkillManager.instance;

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(StateMachine, this, "Idle");
        MoveState = new PlayerMoveState(StateMachine, this, "Move");
        JumpState = new PlayerJumpState(StateMachine, this, "Jump");
        InAirState = new PlayerInAirState(StateMachine, this, "Jump");
        DashState = new PlayerDashState(StateMachine, this, "Dash");
        WallSlideState = new PlayerWallSlideState(StateMachine, this, "WallSlide");
        WallJumpState = new PlayerWallJumpState(StateMachine, this, "Jump");
        PrimaryAttack = new PlayerPrimaryAttackState(StateMachine, this, "Attack_1");
        CounterAttackState = new PlayerCounterAttackState(StateMachine, this, "CounterAttack");
        AimSwordState = new PlayerAimSwordState(StateMachine, this, "SwordAim");
        TackBackSwordState = new PlayerTackBackSwordState(StateMachine, this, "SwordTackBack");
        BlackHoleState = new PlayerBlackHoleState(StateMachine, this, "Jump");
        dieState = new PlayerDieState(StateMachine, this, "Die");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();
        //Debug.Log(StateMachine.currentState);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.skill.crystalSkill.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseFlask();
        }
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(TackBackSwordState);
        Destroy(sword);
    }

    public void CheckForDashInput()
    {
        if (!skill.dash.dashUnlockEnabled) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && this.skill.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            StateMachine.ChangeState(DashState);
        }
    }

    //�˳��ڶ�״̬
    public void ExitBlackHoleState()
    {
        StateMachine.ChangeState(IdleState);
        entityFX.MakeTransparent(false);
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    public void CanBlackHoleReleased(bool canReleased)
    {
        canBlackHoleReleased = canReleased;
    }

    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(dieState);
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        animator.speed = animator.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
}