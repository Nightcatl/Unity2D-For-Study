using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHideState : EnemyState
{
    private Enemy_Goblin enemy;

    public GoblinHideState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.SeekPlayerPrefab.SetActive(true);

        enemy.SeekPlayer = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SeekPlayerPrefab.SetActive(false);

        enemy.FindPlayer = false;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.FindPlayer)
            stateMachine.ChangeState(enemy.idleState);
    }
}
