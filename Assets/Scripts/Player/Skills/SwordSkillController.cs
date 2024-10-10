using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D swordCollider2D;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [SerializeField] private float returnSpeed = 10f;

    [Header("Bounce Sword Info")]
    [SerializeField] private float bounceSpeed;
    //����������һ��ʱ���Զ�����
    private bool isBouncing = true;
    public int bounceCount;
    public List<Transform> enemyTargets;
    [SerializeField] private float bounceTime;
    [SerializeField] private float bounceRadius = 1f;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        bounceCount = player.skill.swordSkill.bounceCount;
    }

    //����Sword����
    public void SetUpSword(Vector2 flyDir, float gravity, Player player)
    {
        this.player = player;
        rb.velocity = flyDir;
        rb.gravityScale = gravity;

        animator.SetBool("Rotation", true);
    }

    //�ý�����
    public void ReturnSword()
    {

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        isBouncing =false;
    }

    //�ý��ĵķ�������й켣һ��
    private void Update()
    {


        if(canRotate) 
            transform.right = rb.velocity;

        if(isReturning)
        {
            //�ý��ص���ҵ�λ��
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //������ص�����ҵ�λ�ã��������
            if (Vector2.Distance(transform.position , player.transform.position) < 1f)
            {
                
                isReturning = false;
                player.CatchTheSword();
            }
        }

        bounceTime -= Time.deltaTime;
        
        //�����Ե���
        if(isBouncing && enemyTargets.Count > 1)
        {
            Debug.Log("Bouncing " + bounceCount);
            if (bounceCount == 0 || bounceTime <= 0)
            {
                ReturnSword();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, enemyTargets[1].position, bounceSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, enemyTargets[1].position) < 0.1f)
                {
                    //����һ����ײ����һ�ε�������
                    bounceCount--;
                    enemyTargets.Clear();
                    UpdateEnemyTargets();
                } 
            }

        }
    }

    private static int triggerCount;
    //���������˻��ǵ���
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (isReturning)
        {
            return;
        }

        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing  && enemyTargets.Count <= 0)
            {
                UpdateEnemyTargets();
            }
        }
        

        SwordBack(other);
    }

    //ÿ����һ�ε��˾͸���һ�ε����б�
    private void UpdateEnemyTargets()
    {
        Collider2D[] surroundEnemys = Physics2D.OverlapCircleAll(transform.position, 10, enemyLayer);
        foreach (Collider2D item in surroundEnemys)
        {
            if (item.GetComponent<Enemy>() != null)
            {
                enemyTargets.Add(item.transform);
            }

        }

        //��⵽���ֻ��һ�����˾Ͳ�������ֱ�Ӳ���ȥ
        if (enemyTargets.Count == 1)
            isBouncing = false;
        else
            enemyTargets = enemyTargets.OrderBy(x => Vector2.Distance(transform.position, x.position)).ToList<Transform>();
    }

    private void SwordBack(Collider2D other)
    {
        Debug.Log("�����������ˣ�" + other.name);
        canRotate = false;
        swordCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //�Ƶ��������Ϊ�˽��������˺���Ȼ����ת�����Ҳ�����ڵ�����
        if (isBouncing)
        {
            return;
        }
        animator.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}
