using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDeadState : EnemyState
{
    Enemy_Goblin enemy;

    public GoblinDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.IsDead = true;

        enemy.CollideCheckPrefab.SetActive(false);
        enemy.SeekPlayerPrefab.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.deathState);
    }
}
