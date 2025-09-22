using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public int comboCounter = 0;

    private float lastTimeAttacked;
    private float combowWindow = 2;

    private float gravityScale;

    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        xInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + combowWindow)
            comboCounter = 0;

        if (stateMachine.lastState is PlayerAirState || stateMachine.lastState is PlayerJumpState)
        {
            comboCounter = 0;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0f;
        }
            
        float attackDir = player.facingDir;

        player.enemies = new List<Enemy>();

        player.anim.SetInteger("ComboCounter", comboCounter);

        if (stateMachine.lastState is not PlayerLimitMoveState && stateMachine.lastState is not PlayerAirState && stateMachine.lastState is not PlayerJumpState)
            player.SetVelocityForAttack(player.attackMovement[comboCounter].x * attackDir, player.facingDir);


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;

        if(rb.gravityScale == 0f)
            rb.gravityScale = gravityScale;

        player.enemies = new List<Enemy>();
    }

    public override void Update()
    {
       base.Update();

       if(stateTimer < 0 && player.rb.velocity.x != 0)
       {
            player.SetZeroVelocity();
       }

       if(triggerCalled)
       {
            switch(stateMachine.lastState)
            {
                case PlayerLimitMoveState limitMoveState:
                    stateMachine.ChangeState(player.limitMoveState);
                    break;
                case PlayerIdleState idleState:
                    stateMachine.ChangeState(player.idleState);
                    break;
                case PlayerJumpState jumpState:
                    stateMachine.ChangeState(player.airState);
                    break;
                case PlayerAirState airState:
                    stateMachine.ChangeState(player.airState);
                    break;
                case PlayerAttackState attackState:
                    stateMachine.ChangeState(player.idleState);
                    break;
                case PlayerMoveState moveState:
                    stateMachine.ChangeState(player.idleState);
                    break;
            }
        }
    }
}
