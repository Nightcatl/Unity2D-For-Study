using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKey(KeyCode.Space))
        {
            if (xInput != 0)
                player.SetVelocity(player.moveSpeed * .7f * xInput, rb.velocity.y);

            rb.velocity += Vector2.up * Physics2D.gravity.y * 3f * Time.deltaTime;
        }
        else
        {
           if (xInput != 0)
                player.SetVelocity(player.moveSpeed * .7f * xInput, rb.velocity.y);

            rb.velocity += Vector2.up * Physics2D.gravity.y * 8f * Time.deltaTime;
        }

        if (rb.velocity.y < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.CanAirAttack)
        {
            stateMachine.ChangeState(player.attackState);
            player.CanAirAttack = false;
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }
    }
}
