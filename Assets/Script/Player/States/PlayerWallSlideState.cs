using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    float slidSpeed = -3;

    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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

        player.SetVelocity(0, slidSpeed);

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if(!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) && xInput != player.facingDir)
        {
            if(xInput == -player.facingDir)
            {
                player.SetVelocity(player.moveSpeed * -player.facingDir, rb.velocity.y);
                stateMachine.ChangeState(player.jumpState);
            }

            player.SetVelocity(player.moveSpeed * -player.facingDir * 0.2f, rb.velocity.y);
            stateMachine.ChangeState(player.jumpState);
        }

        if (xInput == -player.facingDir)
        {
            player.SetVelocity(player.moveSpeed * -player.facingDir * 0.5f , 0);
            stateMachine.ChangeState(player.airState);
        }    
    }
}
