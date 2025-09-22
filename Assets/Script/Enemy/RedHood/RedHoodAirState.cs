using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodAirState : EnemyState
{
    private Enemy_RedHood enemy;

    public RedHoodAirState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
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

        enemy.anim.SetFloat("yVelocity", rb.velocity.y);

        if(enemy.IsGroundDetected())
            stateMachine.ChangeState(enemy.landingState);
    }
}
