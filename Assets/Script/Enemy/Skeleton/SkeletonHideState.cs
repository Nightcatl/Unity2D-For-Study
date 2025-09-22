using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SkeletonHideState : EnemyState
{
    Enemy_Skeleton enemy;

    public SkeletonHideState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
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

        if (enemy.FindPlayer)
            stateMachine.ChangeState(enemy.idleState);
    }
}
