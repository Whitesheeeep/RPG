using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    private EntityFX entityFX;

    [Header("Major Stats")]
    public Stats strength;//����������һ���˺������ӱ����˺� 1%
    public Stats agility; //���ݣ����� 1% �����ʣ����� 1% ������
    public Stats intelligence; //���������� 1% ���ף����� 1% ħ��
    public Stats vitality; //���������� 1% ����ֵ������ 1% �����ظ�

    [Header("Offensive Info")]
    public Stats damage;
    public Stats criticalChance;//��������
    public Stats criticalPower; //����


    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;//����ֵ
    public Stats magicResistance;//ħ������

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    [Header("Abnormal Condition")]
    #region �����쳣״̬
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

        criticalPower.SetBaseValue(150);//���ñ����˺�����ֵΪ150%
        currentHealth = GetMaxHealth();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // ���е�״̬��������Ĭ�ϳ���ʱ�䣬�������˾ͽ���״̬
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

        //����ȼ�󣬳��ֶ���˺����ȼֹͣ
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
    /// ���������˺�Ȼ����㣨TakeDamge��
    /// </summary>
    /// <param name="targetStatus">Ҫ�����˺���Ŀ��</param>
    public virtual void DoDamageTo(CharacterStatus targetStatus)
    {
        //�������ܣ�������ܳɹ������˺�
        if(targetStatus.targetCanAvoidAttack(targetStatus))
        {
            Debug.Log("���ܳɹ�");
            return;
        }

        float totalDamege = damage.GetValue() + strength.GetValue();

        if(CanCritical())
        {
            totalDamege = totalDamege *
                (strength.GetValue() + criticalPower.GetValue()) * .01f;
            Debug.Log("�����ɹ�" + totalDamege);
        }

        //���ռ���
        totalDamege = CheckTargetArmor(targetStatus, totalDamege);

        targetStatus.TakeDamage(totalDamege);
        //DoMagicDamageTo(targetStatus);
    }

    //ħ���˺�
    public virtual void DoMagicDamageTo(CharacterStatus targetStatus)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightingDamage = lightingDamage.GetValue();

        float totleMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totleMagicalDamage = CheckTargetResistance(targetStatus, totleMagicalDamage);

        targetStatus.TakeDamage(totleMagicalDamage);

        //��Ԫ��Ч��ȡ�����˺�
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _fireDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //��ֹѭ��������Ԫ���˺�Ϊ0ʱ������ѭ��
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        //Ϊ�˷�ֹ����Ԫ���˺�һ�¶������޷�����Ԫ��Ч��
        //ѭ���жϴ���ĳ��Ԫ��Ч��
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

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//����ȼ�˺���ֵ

    //��������
    private float CheckTargetResistance(CharacterStatus _targetStatus, float totleMagicalDamage)//��������
    {
        totleMagicalDamage -= _targetStatus.magicResistance.GetValue() + (_targetStatus.intelligence.GetValue() * 3);
        totleMagicalDamage = Mathf.Clamp(totleMagicalDamage, 0, int.MaxValue);
        return totleMagicalDamage;
    }

    //����Ŀ�껤��ֵ���������м�ȥ
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

        //Mathf.Clamp() �����������ǽ�����ֵǯ���ڸ�����С�������������ֵ����ķ�Χ֮�䡣�������С�����Χ�ڣ��򷵻ظ���ֵ��
        /*������ֵǯ���ڸ�����С�������������ֵ����ķ�Χ֮�䡣�������С�����Χ�ڣ��򷵻ظ���ֵ��
        �������ֵС����Сֵ���򷵻���Сֵ���������ֵ�������ֵ���򷵻����ֵ��min �� max �����������ڡ����磬Clamp(10, 0, 5) ������������Ϊ 5�������� 4��*/
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    //���㱬��
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
