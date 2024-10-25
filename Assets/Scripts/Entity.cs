using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isBusy { get; private set; } = false;

    public CharacterStatus status;

    #region Collision Info
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField]protected Vector2 KnockbackDir;
    private bool isKnockbacked;

    [HideInInspector] public EntityFX entityFX;
    [SerializeField] protected Transform groundCheck;//SerializeField 使私有变量在Inspector中显示
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    #endregion

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #endregion Components

    //面朝方向
    public int facingDir { get; private set; } = 1;
    private bool IsFaceRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        entityFX = GetComponentInChildren<EntityFX>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        status =  GetComponent<CharacterStatus>();
    }

    protected virtual void Update()
    {

    }
    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnockbacked)
        {
            return;
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion Velocity
    
    #region Collision Check
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        //检查攻击范围
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion Collision Check

    #region Flip
    public virtual void Flip()
    {
        //旋转
        facingDir *= -1;
        IsFaceRight = !IsFaceRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float x)
    {
        if (x == 0) return;
        if (x < 0 && IsFaceRight)
        {
            Flip();
        }
        else if (x > 0 && !IsFaceRight)
        {
            Flip();
        }
    }
    #endregion Flip

    /// <summary>
    /// 展示受伤效果
    /// </summary>
    public void GetDamagedEffects()
    {
        this.StartCoroutine("HitKnockBack");
        entityFX.StartCoroutine("FlashFX");
    }

    //一个协程，让isBusy在等待一段时间后变为false
    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    protected IEnumerator HitKnockBack()
    {
        isKnockbacked = true;
        rb.velocity = new Vector2(KnockbackDir.x * -facingDir, KnockbackDir.y);
        yield return new WaitForSeconds(0.07f);
        isKnockbacked = false;
    }

    public void MakeTransparent(bool beTransparent)
    {
        if (beTransparent)
        {
            sr.color = Color.clear;
        }
        else { sr.color = Color.white; }
    }

    public virtual void Die()
    {

    }
}
