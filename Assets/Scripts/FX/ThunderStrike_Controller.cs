using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStatus targetStatus;
    [SerializeField] private float flySpeed;
    private float damage =5;

    private Animator animator;
    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetUpThunder(float _damage, CharacterStatus _targetStats)
    {
        damage = _damage;
        targetStatus = _targetStats;
    }


    // Update is called once per frame
    void Update()
    {
        if (!targetStatus)
        {
            return;
        }

        if (triggered)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetStatus.transform.position, flySpeed * Time.timeScale);
        transform.right = transform.position - targetStatus.transform.position;

        if (Vector2.Distance(transform.position, targetStatus.transform.position) < 0.1f)
        {
            animator.transform.position = transform.position + new Vector3(0, 1f);
            animator.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            animator.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStatus.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
