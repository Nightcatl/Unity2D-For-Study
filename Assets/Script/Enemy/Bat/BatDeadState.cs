using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDeadState : EnemyState
{
    private Enemy_Bat enemy;

    public BatDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.IsDead = true;
        rb.gravityScale = 1f;

        enemy.GetComponent<CapsuleCollider2D>().isTrigger = false;
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
