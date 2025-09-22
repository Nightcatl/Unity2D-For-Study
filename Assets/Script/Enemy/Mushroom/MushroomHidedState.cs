using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomHidedState : EnemyState
{
    protected Enemy_Mushroom enemy;

    public MushroomHidedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.SeekPlayer = false;

        enemy.GetComponent<CapsuleCollider2D>().enabled = false;
        enemy.rb.gravityScale = 0;
        enemy.sr.sortingLayerName = "Ground";

        enemy.SetVelocity(0, -3);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SeekPlayerPrefab.SetActive(true);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.hideState);
        }  
    }
}
