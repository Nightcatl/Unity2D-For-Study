using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockSuccessState : PlayerState
{
    public PlayerBlockSuccessState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        if (!player.IsOver)
            stateTimer = .3f;
    }

    public override void Exit()
    {
        base.Exit();

        player.isBlockSuccess = false;
        
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0 && stateMachine.currentState is PlayerLimitMoveState)
        {
            SkillManager.instance.water.Retaliation();
            stateMachine.ChangeState(player.limitMoveState);
            return;
        }

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.counterattackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.hitAndAwayState);
            return;
        } 
    }
}
