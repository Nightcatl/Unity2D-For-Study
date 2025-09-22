using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodDeadState : EnemyState
{
    private Enemy_RedHood enemy;

    public RedHoodDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinshTrigger()
    {
        base.AnimationFinshTrigger();

        enemy.bossManager.WinTheFight();
    }

    public override void Enter()
    {
        base.Enter();

        enemyBase.SetZeroVelocity();

        Debug.Log("Enter DeadState");
    }

    public override void Exit()
    {
        base.Exit();


    }
}
