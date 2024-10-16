using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  这个水晶的初始位置在 Player处
/// </summary>
public class CrystalController : MonoBehaviour
{
    /// <summary>
    /// 水晶技能持续时间
    /// </summary>
    private float crystalSkillDuration;
    private bool canExplode;//控制能否爆炸
    private float maxExplosionRadius;
    private float explosionSpeed = 1f;
    private bool canTrace;
    private Transform closestEnemy;
    private float traceSpeed;

    #region Components
    private Animator animator => GetComponentInChildren<Animator>();
    private CircleCollider2D circleCollider2D => GetComponent<CircleCollider2D>();
    #endregion

    public void SetUpCrystal(float crystalSkillDuration, 
        bool canExplode,float maxExplosionRadius, float explosionSpeed,
        bool canTrace, float traceSpeed)
    {
        this.crystalSkillDuration = crystalSkillDuration;
        this.canExplode = canExplode;
        this.maxExplosionRadius = maxExplosionRadius;
        this.explosionSpeed = explosionSpeed;
        this.canTrace = canTrace;
        this.traceSpeed = traceSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        #region 跟随逻辑
        if (canTrace)
        {
            closestEnemy = SkillManager.instance.crystalSkill.FindClosestEnemy(transform);
            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, traceSpeed * Time.deltaTime);
                
            }
            
        }
        #endregion 跟随逻辑

        #region 爆炸逻辑
        crystalSkillDuration -= Time.deltaTime;
        if (crystalSkillDuration <= 0)
        {
            if (canExplode)
            {
                animator.SetBool("Explode", true);
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxExplosionRadius, maxExplosionRadius), explosionSpeed*Time.deltaTime);
            }
            else
            {
                DestroySelf();
            }
        }
        #endregion 爆炸逻辑
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        Debug.Log("Explode: "+circleCollider2D.radius*transform.localScale.x);
        //Collider 会跟随者一起变化，所以不用管 Collider
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius*transform.localScale.x);//使用Collider的半径即可随着变化而变化
        foreach (var item in colliders)
        {
            if (item.GetComponent<Enemy>() != null)
            {
                item.GetComponent<Enemy>()?.GetDamaged();
            }
        }
    }

    //让Duration 直接归零
    public void DurationTobeZero() => crystalSkillDuration = 0;
}
