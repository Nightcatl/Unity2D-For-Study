using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodJumpState: EnemyState
{
    private Enemy_RedHood enemy;

    public RedHoodJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.jumpDir == -1)
            enemy.SetVelocityForAttack(enemy.moveSpeed * 0.6f * enemy.jumpDir * enemy.facingDir, enemy.jumpForce);
        else
            enemy.SetVelocity(enemy.moveSpeed * 0.6f * enemy.jumpDir * enemy.facingDir, enemy.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.airState);
    }
}
