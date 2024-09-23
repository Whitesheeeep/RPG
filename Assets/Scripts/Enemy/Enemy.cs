using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    [HideInInspector] public float LastAttackTime;
    public float battleTime;
    public float escapeBattleDistance;
    #region ��
    public EnemyStateMachine stateMachine;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }
    protected override void Start()
    {
        base.Start();

    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        //Debug.Log(IsPlayerDetected().collider.gameObject.name + "I see");//�⴮����ᱨ������ʹ�汾�����壬��Ϊ��û���ҵ�Player��ʱ�������ǿյģ�NULL�����������ڿ���̨����ʾ�ͱ�����
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 15, whatIsPlayer);//���ڴ�����Ͷ���ȡ��Ϣ�Ľṹ��
                                                                                                                                         //�ú����ķ���ֵ���Ա䣬����ֻ����bool��Ҳ�����������Ľṹ
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}