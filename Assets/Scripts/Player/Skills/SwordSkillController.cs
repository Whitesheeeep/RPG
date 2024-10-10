using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Bounce Sword Info")]
    [SerializeField] private float bounceSpeed;
    //剑弹跳超过一段时间自动返回
    private bool isBouncing = true;
    public int bounceCount;
    public List<Transform> enemyTargets;
    [SerializeField] private float bounceTime;
    [SerializeField] private float bounceRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        bounceCount = player.skill.swordSkill.bounceCount;
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
        isBouncing =false;
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

        bounceTime -= Time.deltaTime;
        
        //剑可以弹跳
        if(isBouncing && enemyTargets.Count > 1)
        {
            Debug.Log("Bouncing " + bounceCount);
            if (bounceCount == 0 || bounceTime <= 0)
            {
                ReturnSword();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, enemyTargets[1].position, bounceSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, enemyTargets[1].position) < 0.1f)
                {
                    //进行一次碰撞减少一次弹跳次数
                    bounceCount--;
                    enemyTargets.Clear();
                    UpdateEnemyTargets();
                } 
            }

        }
    }

    private static int triggerCount;
    //剑碰到敌人或是地面
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (isReturning)
        {
            return;
        }

        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing  && enemyTargets.Count <= 0)
            {
                UpdateEnemyTargets();
            }
        }
        

        SwordBack(other);
    }

    //每碰到一次敌人就更新一次敌人列表
    private void UpdateEnemyTargets()
    {
        Collider2D[] surroundEnemys = Physics2D.OverlapCircleAll(transform.position, 10, enemyLayer);
        foreach (Collider2D item in surroundEnemys)
        {
            if (item.GetComponent<Enemy>() != null)
            {
                enemyTargets.Add(item.transform);
            }

        }

        //检测到如果只有一个敌人就不弹跳，直接插上去
        if (enemyTargets.Count == 1)
            isBouncing = false;
        else
            enemyTargets = enemyTargets.OrderBy(x => Vector2.Distance(transform.position, x.position)).ToList<Transform>();
    }

    private void SwordBack(Collider2D other)
    {
        Debug.Log("触发器触发了：" + other.name);
        canRotate = false;
        swordCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //移到这里就是为了剑碰到敌人后仍然是旋转动画且不会黏在敌人上
        if (isBouncing)
        {
            return;
        }
        animator.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}
