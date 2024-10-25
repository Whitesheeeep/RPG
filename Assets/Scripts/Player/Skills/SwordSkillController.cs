using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class SwordSkillController : MonoBehaviour
{
    #region Component
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D swordCollider2D;
    private Player player;
    #endregion Component

    private bool canRotate = true;
    private bool isReturning;

    private float returnSpeed = 10f;
    private float freezeTime;

    [Header("Pierce Info")]
    private int pierceCount;

    #region Bounce Sword Info
    [Header("Bounce Sword Info")]
    private float bounceSpeed;
    //剑弹跳超过一段时间自动返回
    private bool isBouncing;
    private int bounceCount;
    public List<Transform> enemyTargets;
    public LayerMask enemyLayer;
    #endregion Bounce Sword Info

    #region Spin Info
    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinnning;
    private float spinDir;

    private float damageTimer;
    private float damageCooldown = 0.5f;
    #endregion Spin Info

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    #region SetUpSword
    //控制Sword飞行
    public void SetUpSword(Vector2 flyDir, float gravity, Player player, float freezeTime, float returnSpeed)
    {
        this.player = player;
        rb.velocity = flyDir;
        rb.gravityScale = gravity;
        this.freezeTime = freezeTime;
        this.returnSpeed = returnSpeed;

        if(pierceCount <= 0 ) 
            animator.SetBool("Rotation", true);

        spinDir = Mathf.Clamp(rb.velocity.x , -1,1);
    }

    public void SetUpBounce(bool isBouncing, int bounceCount, float bounceSpeed)
    {
        this.isBouncing = isBouncing;
        this.bounceCount = bounceCount;
        this.bounceSpeed = bounceSpeed;
    }

    public void SetUpPierce(int pierceCount)
    {
        this.pierceCount = pierceCount;
    }

    public void SetUpSpin(float maxTravelDistance, float spinDuration, bool isSpinning, float damageCD)
    {
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.isSpinnning = isSpinning;
        this.damageCooldown = damageCD;
    }
    #endregion  SetUpSword

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
        Debug.Log("isBouncing " + isBouncing);

        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            //让剑回到玩家的位置
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //如果剑回到了玩家的位置，就清除剑
            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
            {

                isReturning = false;
                player.CatchTheSword();
            }
        }

        //bounceTime -= Time.deltaTime;
        Debug.Log("isBounce"+isBouncing);//false
        //剑可以弹跳
        if (isBouncing && enemyTargets.Count > 1)
        {
            BounceStart();

        }
        Debug.Log("敌人数量："+enemyTargets.Count);
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinnning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer <= 0)
                {
                    wasStopped = false;
                    isReturning = true;
                    isSpinnning = false;
                }
            }

            damageTimer -= Time.deltaTime;
            if (damageTimer < 0)
            {
                damageTimer = damageCooldown;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1, enemyLayer);
                foreach (Collider2D hit in hitEnemies)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();
                        DamageEnemy(enemy);
                    }
                }
            }
        }
    }

    private void BounceStart()
    {
        Debug.Log("弹跳次数： " + bounceCount);
        if (bounceCount == 0)
        {
            ReturnSword();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[1].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[1].position) < 0.1f)
            {
                Enemy enemy = enemyTargets[1].GetComponent<Enemy>();
                DamageEnemy(enemy);
                //进行一次碰撞减少一次弹跳次数
                bounceCount--;
                enemyTargets.Clear();
                UpdateEnemyTargets();
            }
        }
    }
    private void DamageEnemy(Enemy enemy)
    {
        enemy?.GetDamagedEffects();
        //enemy.StartCoroutine("FreezeTimeFor", freezeTime);
    }

    
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }


    //剑碰到敌人或是地面
    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (isReturning)
        {
            return;
        }

        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                UpdateEnemyTargets();
            }

            Enemy enemy = other.GetComponent<Enemy>();
            enemy?.GetDamagedEffects();
            enemy.StartCoroutine("FreezeTimeFor", freezeTime);
        }
        

        SwordBack(other);
    }

    //每碰到一次敌人就更新一次敌人列表
    private void UpdateEnemyTargets()
    {
        Collider2D[] surroundEnemys = Physics2D.OverlapCircleAll(transform.position, 10, enemyLayer);
        Debug.Log("碰撞得到的敌人数量："+surroundEnemys.Length);
        foreach (Collider2D item in surroundEnemys)
        {
            
            if (item.GetComponent<Enemy>() != null)
            {
                enemyTargets.Add(item.transform);
            }

        }

        //检测到如果只有一个敌人就不弹跳，直接插上去
        if (enemyTargets.Count == 1)
        {
            Debug.Log("isBouncing " + isBouncing);
            isBouncing = false;
        }
        else
            enemyTargets = enemyTargets.OrderBy(x => Vector2.Distance(transform.position, x.position)).ToList<Transform>();
    }

    private void SwordBack(Collider2D other)
    {
        //如果是穿透剑，碰到敌人后不会停止旋转
        if (pierceCount > 0 && other.GetComponent<Enemy>() != null)
        {
            pierceCount--;
            return;
        }

        if (isSpinnning)
        {
            //遇到第一个敌人时直接开始旋转，而不是达到目的地才开始转
            //StopWhenSpinning();
            return;
        }

        Debug.Log("触发器触发了：" + other.name);
        canRotate = false;
        swordCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //移到这里就是为了剑碰到敌人后仍然是旋转动画且不会黏在敌人上
        if (isBouncing && enemyTargets.Count > 0)
        {
            return;
        }
        animator.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

    
}
