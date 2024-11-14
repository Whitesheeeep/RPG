using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashAttack_Controller : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("FlashAttack");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();
            playerStatus.DoMagicDamageTo(enemyStatus);
        }
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
