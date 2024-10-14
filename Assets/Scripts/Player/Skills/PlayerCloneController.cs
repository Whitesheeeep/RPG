using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[DefaultExecutionOrder(100)]
public class PlayerCloneController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private float cloneExistTimer;
    private float cloneFadingSpeed;

    private Player player;

    private float facingDir = 1;

    [SerializeField] private Transform attackCheck;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        player = PlayerManager.instance.player;
        //控制一开始生成的影子的朝向
        if (player.facingDir < 0 )
        {
            transform.Rotate(0, 180, 0);
            facingDir = -1;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        cloneExistTimer -= Time.deltaTime;
        if (cloneExistTimer <= 0)
        {
            sr.color = new Color(1,1,1, sr.color.a - (Time.deltaTime * cloneFadingSpeed));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetUpPlayerClone(Transform newTransform, float cloneExistDuration, float cloneFadingSpeed, bool canAttack, float offset = 0f )
    {
        if (canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));//从三个攻击中随机选择一个
        }

        transform.position = newTransform.position + new Vector3(offset, 0, 0);
        cloneExistTimer = cloneExistDuration;
        this.cloneFadingSpeed = cloneFadingSpeed;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneExistTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCheck.position, player.attackRadius);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().GetDamaged();
            }
        }
    }

    //TODO: 朝向最近的敌人
    private void FaceClosestTarget()
    {
        Collider2D[] enemyDetected = Physics2D.OverlapCircleAll(transform.position, 20, LayerMask.GetMask("Enemy"));
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        
        foreach (Collider2D hit in enemyDetected)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        if (closestEnemy != null)
        {
            if (closestEnemy.position.x < transform.position.x)
            {
                if (facingDir != -1)
                {
                    transform.Rotate(0, 180, 0);
                }
            }
            else if (closestEnemy.position.x > transform.position.x )
            {
                if (facingDir == -1)
                {
                    transform.Rotate(0, 180, 0);
                }
            }
        }
        
    }
}
