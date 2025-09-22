using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodDashState : EnemyState
{
    private Enemy_RedHood enemy;

    public RedHoodDashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        stateTimer = enemy.dashTime;
        enemy.SetVelocity(enemy.moveSpeed * 1.5f * enemy.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        if(!enemy.CheckPlayerFacing())
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * 1.5f * enemy.facingDir, rb.velocity.y);

        if(stateTimer <= 0)
            stateMachine.ChangeState(enemy .idleState);
    }
}
