using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodArcheryState : EnemyState
{
    private Enemy_RedHood enemy;

    public RedHoodArcheryState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void AnimationFinshTrigger()
    {
        base.AnimationFinshTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        if (PlayerManager.instance.player.transform.position.y > enemy.transform.position.y)
            enemy.archeryType = 1;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.archeryType = 0;
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(enemy.idleState);
    }
}
