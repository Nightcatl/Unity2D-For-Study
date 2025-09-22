using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerState
{
    public PlayerBlockState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();

        stateTimer = player.blockTime;
        player.isBlock = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.isBlock = false;
    }

    public override void Update()
    {
        base.Update();

        if(player.isBlockSuccess)
        {
            stateMachine.ChangeState(player.blockSuccessState);
            return;
        }
           

        if (stateTimer <= 0)
        {
            if(stateMachine.currentState is PlayerLimitMoveState)
                stateMachine.ChangeState(player.limitMoveState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }
}
