using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;//����������һ���˺������ӱ����˺� 1%
    public Stats agility; //���ݣ����� 1% �����ʣ����� 1% ������
    public Stats intelligence; //���������� 1% ���ף����� 1% ħ��
    public Stats endurance; //���������� 1% ����ֵ������ 1% �����ظ�

    [Header("Offensive Info")]
    public Stats damage;
    public Stats criticalChance;//��������
    public Stats criticalPower; //����


    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;//����ֵ


    public float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        criticalPower.SetBaseValue(150);//���ñ����˺�����ֵΪ150%
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
    }

    //����Ŀ�껤��ֵ���������м�ȥ
    private float CheckTargetArmor(CharacterStatus targetStatus, float totalDamage)
    {
        totalDamage -= targetStatus.armor.GetValue();

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
