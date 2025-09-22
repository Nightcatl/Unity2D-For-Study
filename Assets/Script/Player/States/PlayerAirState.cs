using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float stuckTimer;
    private bool CanStuck;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.lastState == player.jumpState)
        {
            CanStuck = true;
        }
        else
        {
            CanStuck = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //�Ϳ�
        if(CanStuck && stuckTimer > 0 && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);

            stuckTimer -= Time.deltaTime;

            if(stuckTimer < 0)
            {
                CanStuck = false;
            }
        }

        //����ʱ��
        if(Input.GetKeyDown(KeyCode.Space) && Time.time - player.lastInGoundTime <= player.coyoteTime && stateMachine.lastState != player.jumpState)
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        //�����ո�
        if(stateTimer > 0 && Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }

        //�ɿ��ո�
        if(!Input.GetKey(KeyCode.Space) || rb.velocity.y <= 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 4f * Time.deltaTime;
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        //���й���
        if(Input.GetKeyDown(KeyCode.Mouse0) && player.CanAirAttack)
        {
            stateMachine.ChangeState(player.attackState);
            player.CanAirAttack = false;
        }
        
        //�����ƶ�
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .7f * xInput, rb.velocity.y);

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
    }
}
