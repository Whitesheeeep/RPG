using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isBusy { get; private set; } = false;

    #region Collision Info
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected Transform groundCheck;//SerializeField ʹ˽�б�����Inspector����ʾ
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    #endregion

    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion Components

    //�泯����
    public int facingDir { get; private set; } = 1;
    private bool IsFaceRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }
    #region Velocity
    public void SetVelocity(float xVelocity, float yVelocity)
    {
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

        //��鹥����Χ
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion Collision Check

    #region Flip
    public virtual void Flip()
    {
        //��ת
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

    public void GetDamaged()
    {
        Debug.Log(gameObject.name + "  �ܵ��˺�");
    }

    //һ��Э�̣���isBusy�ڵȴ�һ��ʱ����Ϊfalse
    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }
}
