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
        //如果已经有一个剑了，无法进入状态
        /*在 Unity 中，GameObject 如果为空（即为 null），可以直接将其转换为 bool 值。
         * Unity 对 GameObject 和其他 UnityEngine 对象进行了特殊处理，使得它们可以直接用于布尔上下文中。
         * 具体来说，如果 GameObject 为 null，转换为 bool 值时会返回 false；如果不为 null，则返回 true。*/
        //利用短路运算符，如果player.sword为空，直接返回true，不会执行后面的代码，按下第二次F时，
        //才会执行 HasNoSword() 方法，执行ReturnSword()方法
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

        //如果玩家已经有一个剑了，无法进入状态
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
