using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBreakState : EnemyState
{
    protected Enemy_Mushroom enemy;

    public MushroomBreakState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.IsBreak = false;
        enemy.stat.currentTenacity = enemy.stat.Tenacity.GetValue();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(enemy.idleState);     
    }
}
