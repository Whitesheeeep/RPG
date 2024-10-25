using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;//力量，增加一点伤害，增加暴击伤害 1%
    public Stats agility; //敏捷，增加 1% 闪避率，增加 1% 暴击率
    public Stats intelligence; //智力，增加 1% 护甲，增加 1% 魔抗
    public Stats endurance; //耐力，增加 1% 生命值，增加 1% 生命回复

    [Header("Offensive Info")]
    public Stats damage;
    public Stats criticalChance;//暴击概率
    public Stats criticalPower; //爆伤


    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;//闪避值


    public float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        criticalPower.SetBaseValue(150);//设置暴击伤害基础值为150%
        currentHealth = maxHealth.GetValue();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(currentHealth <= 0)
        {
            //Die
            //Destroy(gameObject);
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
    }

    //计算目标护甲值并从总伤中减去
    private float CheckTargetArmor(CharacterStatus targetStatus, float totalDamage)
    {
        totalDamage -= targetStatus.armor.GetValue();

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
        float randomValue = Random.Range(0, 100);
        if (randomValue <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;

        Debug.Log(this.GetType().Name + " take damage: " + damageValue + " current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private bool targetCanAvoidAttack(CharacterStatus targetStatus)
    {
        float totalEvasion = targetStatus.evasion.GetValue() + targetStatus.agility.GetValue();
        float randomValue = Random.Range(0, 100);
        if(randomValue <= totalEvasion)
        {
            return true;
        }

        return false;
    }

    public virtual void Die()
    {

    }
}
