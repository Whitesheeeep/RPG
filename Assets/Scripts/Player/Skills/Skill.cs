using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// �жϷ�ʹ�ü��ܣ��������ʹ�ü��������UseSkill()����
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log($"{this.GetType().Name} is on cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //do some skill specific things
    }

    /// <summary>
    /// ����Ѱ������ĵ���
    /// </summary>
    /// <param name="objectTransform">������Ҫ���ҵ�����з���λ�Ķ���</param>
    /// <returns>����ĵз���λ�� Transform</returns>
    public Transform FindClosestEnemy(Transform objectTransform)
    {
        Collider2D[] enemyDetected = Physics2D.OverlapCircleAll(objectTransform.position, 20);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D hit in enemyDetected)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distance = Vector2.Distance(objectTransform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                } 
            }
        }

        return closestEnemy;
    }
}
