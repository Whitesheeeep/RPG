using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    private EntityFX entityFX;

    [Header("Major Stats")]
    public Stats strength;//力量，增加一点伤害，增加暴击伤害 1%
    public Stats agility; //敏捷，增加 1% 闪避率，增加 1% 暴击率
    public Stats intelligence; //智力，增加 1% 护甲，增加 1% 魔抗
    public Stats vitality; //耐力，增加 1% 生命值，增加 1% 生命回复

    [Header("Offensive Info")]
    public Stats damage;
    public Stats criticalChance;//暴击概率
    public Stats criticalPower; //爆伤


    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;//闪避值
    public Stats magicResistance;//魔法抗性

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    [Header("Abnormal Condition")]
    #region 各类异常状态
    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;
    #endregion

    [SerializeField] private float alimentsDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private float igniteDamage;
    private float shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    public float currentHealth;
    public Action OnHealthChanged;
    protected bool isDead;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        entityFX = GetComponentInChildren<EntityFX>();

        criticalPower.SetBaseValue(150);//设置暴击伤害基础值为150%
        currentHealth = GetMaxHealth();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 所有的状态都设置上默认持续时间，持续过了就结束状态
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;

        //被点燃后，出现多段伤害后点燃停止
        if(isIgnited)
            ApplyIgniteDamage();
    }

    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0 )
        {
            Debug.Log("Take Burning Damage" + igniteDamage);
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
                Die();
            ignitedDamageTimer = igniteDamageCooldown;
        }
    }

    /// <summary>
    /// 计算所有伤害然后结算（TakeDamge）
    /// </summary>
    /// <param name="targetStatus">要遭受伤害的目标</param>
    public virtual void DoDamageTo(CharacterStatus targetStatus)
    {
        //计算闪避，如果闪避成功则不受伤害
        if(targetStatus.targetCanAvoidAttack(targetStatus))
        {
            Debug.Log("闪避成功");
            return;
        }

        float totalDamege = damage.GetValue() + strength.GetValue();

        if(CanCritical())
        {
            totalDamege = totalDamege *
                (strength.GetValue() + criticalPower.GetValue()) * .01f;
            Debug.Log("暴击成功" + totalDamege);
        }

        //最终减伤
        totalDamege = CheckTargetArmor(targetStatus, totalDamege);

        targetStatus.TakeDamage(totalDamege);
        //DoMagicDamageTo(targetStatus);
    }

    //魔法伤害
    public virtual void DoMagicDamageTo(CharacterStatus targetStatus)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightingDamage = lightingDamage.GetValue();

        float totleMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totleMagicalDamage = CheckTargetResistance(targetStatus, totleMagicalDamage);

        targetStatus.TakeDamage(totleMagicalDamage);

        //让元素效果取决与伤害
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _fireDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //防止循环在所有元素伤害为0时出现死循环
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        //为了防止出现元素伤害一致而导致无法触发元素效果
        //循环判断触发某个元素效果
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            float rnd = UnityEngine.Random.value;
            if (rnd < .25f)
            {
                canApplyIgnite = true;
                Debug.Log("Ignited");
                targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (rnd < .35f)
            {
                canApplyChill = true;
                Debug.Log("Chilled");
                targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (rnd < .55f)
            {
                canApplyShock = true;
                Debug.Log("Shocked");
                targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }

        if (canApplyIgnite)
        {
            
            targetStatus.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        targetStatus.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }


    public virtual void TakeDamage(float damageValue)
    {
        DecreaseHealthBy(damageValue);

        GetComponent<Entity>().GetDamagedEffects();
        entityFX.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(float _damage)
    {
        currentHealth -= _damage;

        OnHealthChanged?.Invoke();
    }

    public void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_isIgnited  && canApplyIgnite)
        {
            isIgnited = _isIgnited;
            ignitedTimer = alimentsDuration;

            Debug.Log("IgniteFX used");
            entityFX.IgniteFxFor(alimentsDuration);
        }
        if (_isChilled && canApplyChill)
        {
            isChilled = _isChilled;
            chilledTimer = alimentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, alimentsDuration);
            entityFX.ChillFxFor(alimentsDuration);
        }
        if (_isShocked && canApplyShock)
        {
            if (!isShocked)
            {
                isShocked = _isShocked;
                shockedTimer = alimentsDuration;

                entityFX.ShockFxFor(alimentsDuration);
            }
            else
            {
                Collider2D[] enemyDetected = Physics2D.OverlapCircleAll(transform.position, 20, LayerMask.GetMask("Enemy"));
                Debug.Log(enemyDetected.Length);
                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;

                foreach (Collider2D hit in enemyDetected)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        float distance = Vector2.Distance(transform.position, hit.transform.position);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestEnemy = hit.transform;
                        }
                    }
                }

                if (closestEnemy != null)
                {
                    GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position + new Vector3(0, 1f), Quaternion.identity);
                    newShockStrike.GetComponent<ThunderStrike_Controller>().SetUpThunder(shockDamage, closestEnemy.GetComponent<CharacterStatus>());
                }
            }

        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//给点燃伤害赋值

    //法抗计算
    private float CheckTargetResistance(CharacterStatus _targetStatus, float totleMagicalDamage)//法抗计算
    {
        totleMagicalDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totleMagicalDamage = Mathf.Clamp(totleMagicalDamage, 0, int.MaxValue);
        return totleMagicalDamage;
    }

    //计算目标护甲值并从总伤中减去
    private float CheckTargetArmor(CharacterStatus targetStatus, float totalDamage)
    {
        if (targetStatus.isChilled)
        {
            totalDamage -= targetStatus.armor.GetValue() * .8f;
        }
        else
        {
            totalDamage -= targetStatus.armor.GetValue(); 
        }

        //Mathf.Clamp() 方法的作用是将给定值钳制在给定最小整数和最大整数值定义的范围之间。如果在最小和最大范围内，则返回给定值。
        /*将给定值钳制在给定最小整数和最大整数值定义的范围之间。如果在最小和最大范围内，则返回给定值。
        如果给定值小于最小值，则返回最小值。如果给定值大于最大值，则返回最大值。min 和 max 参数包括在内。例如，Clamp(10, 0, 5) 将返回最大参数为 5，而不是 4。*/
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    //计算爆伤
    private bool CanCritical()
    {
        float totalCriticalChance = criticalChance.GetValue() + agility.GetValue();
        float randomValue = UnityEngine.Random.Range(0, 100);
        if (randomValue <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }



    private bool targetCanAvoidAttack(CharacterStatus targetStatus)
    {
        float totalEvasion = targetStatus.evasion.GetValue() + targetStatus.agility.GetValue();

        if (isShocked)
        {
            totalEvasion *= 1.2f;
        }

        float randomValue = UnityEngine.Random.Range(0, 100);
        if(randomValue <= totalEvasion)
        {
            return true;
        }

        return false;
    }

    public virtual float GetMaxHealth() => maxHealth.GetValue() + vitality.GetValue() * 5;
    public virtual void Die()
    {
        isDead = true;
    }
}
