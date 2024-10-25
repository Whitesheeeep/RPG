using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public Stats maxHealth;
    public Stats damage;
    public Stats strength;

    public float currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        float totalDamege = damage.GetValue() + strength.GetValue();

        targetStatus.TakeDamage(totalDamege);
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

    public virtual void Die()
    {

    }
}
