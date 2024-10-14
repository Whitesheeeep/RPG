using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //����Ѿ���һ�����ˣ��޷�����״̬
        /*�� Unity �У�GameObject ���Ϊ�գ���Ϊ null��������ֱ�ӽ���ת��Ϊ bool ֵ��
         * Unity �� GameObject ������ UnityEngine ������������⴦��ʹ�����ǿ���ֱ�����ڲ����������С�
         * ������˵����� GameObject Ϊ null��ת��Ϊ bool ֵʱ�᷵�� false�������Ϊ null���򷵻� true��*/
        //���ö�·����������player.swordΪ�գ�ֱ�ӷ���true������ִ�к���Ĵ��룬���µڶ���Fʱ��
        //�Ż�ִ�� HasNoSword() ������ִ��ReturnSword()����
        if (Input.GetKeyDown(KeyCode.F) && HasNoSword())
        {
            stateMachine.ChangeState(player.AimSwordState);
        }
        
        if (Input.GetKeyDown(KeyCode.R) && player.canBlackHoleReleased)
        {
            stateMachine.ChangeState(player.BlackHoleState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.PrimaryAttack);
            
        }

        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.InAirState);
        }
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) 
        { 
            stateMachine.ChangeState(player.JumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.CounterAttackState);
        }


    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        //�������Ѿ���һ�����ˣ��޷�����״̬
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
