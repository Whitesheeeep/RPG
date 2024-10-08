using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        swordCollider2D = GetComponent<CircleCollider2D>();
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
    }

    //���������˻��ǵ���
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }
        Debug.Log("�����������ˣ�"+other.name);
        animator.SetBool("Rotation", false);
        canRotate = false;
        swordCollider2D.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = other.transform;
        Debug.Log("Fuck");
    }
}
