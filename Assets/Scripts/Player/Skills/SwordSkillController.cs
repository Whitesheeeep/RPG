using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D swordCollider2D;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [SerializeField] private float returnSpeed = 10f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider2D = GetComponent<CircleCollider2D>();
    }

    //控制Sword飞行
    public void SetUpSword(Vector2 flyDir, float gravity, Player player)
    {
        this.player = player;
        rb.velocity = flyDir;
        rb.gravityScale = gravity;

        animator.SetBool("Rotation", true);
    }

    //让剑回来
    public void ReturnSword()
    {

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    //让剑的的方向与飞行轨迹一致
    private void Update()
    {
        if(canRotate) 
            transform.right = rb.velocity;

        if(isReturning)
        {
            //让剑回到玩家的位置
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //如果剑回到了玩家的位置，就清除剑
            if (Vector2.Distance(transform.position , player.transform.position) < 1f)
            {
                
                isReturning = false;
                player.CatchTheSword();
            }
        }
    }

    //剑碰到敌人或是地面
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }
        Debug.Log("触发器触发了："+other.name);
        animator.SetBool("Rotation", false);
        canRotate = false;
        swordCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = other.transform;
        Debug.Log("Fuck");
    }
}
