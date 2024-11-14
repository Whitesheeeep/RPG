using System.Collections;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected LayerMask whatIsPlayer;

    #region Stunned Info
    [Header("Stunned Info")]
    public Vector2 stunnedDir;
    public float stunnedTime;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;
    #endregion

    #region Move Info
    [Header("Move Info")]
    public float moveSpeed;
    private float defaultMoveSpeed;
    public float idleTime;
    #endregion

    #region Attack Info
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;

    [HideInInspector] public float LastAttackTime;
    public float battleTime;
    public float escapeBattleDistance;
    #endregion

    #region Die Info
    [Header("Die Info")]
    public float fadeSpeed;
    #endregion

    #region 类
    public EnemyStateMachine stateMachine;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }
    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        //Debug.Log(IsPlayerDetected().collider.gameObject.name + "I see");//这串代码会报错，可能使版本的物体，因为在没有找到Player的时候物体是空的，NULL，你想让他在控制台上显示就报错了
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 15, whatIsPlayer);//用于从射线投射获取信息的结构。
                                                                                                                                         //该函数的返回值可以变，可以只返回bool，也可以是碰到的结构
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    #region CounterMethods
    public virtual void OpenCounterAttackWindow()
    {
        counterImage.SetActive(true);
        canBeStunned = true;
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }
    #endregion

    public void FreezeTimeFor(float _freezeDuration) => StartCoroutine(FreezeTimeCoroutine(_freezeDuration));
    public IEnumerator FreezeTimeCoroutine(float time)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(time);
        FreezeTime(false);
    }

    public void FreezeTime(bool isFreeze)
    {
        if (isFreeze)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    public override void Die()
    {
        base.Die();
        
    }

    public void DestroySelf(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        animator.speed = animator.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration); 
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        animator.speed = 1;
    }
}