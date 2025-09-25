using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterattackState : PlayerState
{
    public PlayerCounterattackState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        player.ColorIndex = 0;

        player.StartFx(0, player.transform.position + new Vector3(-1, 0.8f, 0));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
