using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.CanAirAttack = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            player.lastInGoundTime = Time.time;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }
            

        if(Input.GetKeyDown(KeyCode.Mouse0) && !player.useSkill)
        {
            stateMachine.ChangeState(player.attackState);
            return;
        }
            

        if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(player.rollingState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.blockstate);
            return;
        }
    }
}
