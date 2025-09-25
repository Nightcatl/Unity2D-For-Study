using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitAndAwayState : PlayerState
{
    private bool jumpOnce;

    public PlayerHitAndAwayState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.awayDuration;

        jumpOnce = true ;

        player.attackState.comboCounter = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && jumpOnce)
        {
            player.SetVelocityForAttack(player.awaySpeed.x * -player.facingDir, player.awaySpeed.y);
            jumpOnce = false;
        }

        if(player.IsGroundDetected() && stateTimer <= 0 && triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
