using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAwakeState : EnemyState
{
    protected Enemy_Mushroom enemy;

    public MushroomAwakeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.SetVelocity(0, 3);

        enemy.SeekPlayerPrefab.SetActive(false);

        enemy.sr.sortingLayerName = "Enemy";
    }

    public override void Exit()
    {
        base.Exit();

        enemy.FindPlayer = false;
        enemy.GetComponent<CapsuleCollider2D>().enabled = true;
        enemy.rb.gravityScale = 2;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
           stateMachine.ChangeState(enemy.idleState);
    }
}
