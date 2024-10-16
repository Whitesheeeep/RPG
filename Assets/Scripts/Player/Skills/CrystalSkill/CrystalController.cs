using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ���ˮ���ĳ�ʼλ���� Player��
/// </summary>
public class CrystalController : MonoBehaviour
{
    /// <summary>
    /// ˮ�����ܳ���ʱ��
    /// </summary>
    private float crystalSkillDuration;
    private bool canExplode;//�����ܷ�ը
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
        #region �����߼�
        if (canTrace)
        {
            closestEnemy = SkillManager.instance.crystalSkill.FindClosestEnemy(transform);
            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, traceSpeed * Time.deltaTime);
                
            }
            
        }
        #endregion �����߼�

        #region ��ը�߼�
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
        #endregion ��ը�߼�
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        Debug.Log("Explode: "+circleCollider2D.radius*transform.localScale.x);
        //Collider �������һ��仯�����Բ��ù� Collider
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius*transform.localScale.x);//ʹ��Collider�İ뾶�������ű仯���仯
        foreach (var item in colliders)
        {
            if (item.GetComponent<Enemy>() != null)
            {
                item.GetComponent<Enemy>()?.GetDamaged();
            }
        }
    }

    //��Duration ֱ�ӹ���
    public void DurationTobeZero() => crystalSkillDuration = 0;
}
