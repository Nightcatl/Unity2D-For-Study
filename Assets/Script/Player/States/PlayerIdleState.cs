using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.idleState)
            return;

        if (xInput == player.facingDir && player.IsWallDetected())
            return;

        if (xInput != 0)
        {
            stateMachine.ChangeState(player.moveState);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.magicType.attributeType = AttributeType.Thunder;
            player.magicType.magicNum = 2;
            stateMachine.ChangeState(player.usingMagicState);
        }    

        if(player.useSkill)
        {
            stateMachine.ChangeState(player.usingMagicState);
        }
    }
}
