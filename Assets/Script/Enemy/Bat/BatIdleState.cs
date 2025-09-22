using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BatIdleState : EnemyState
{
    protected Enemy_Bat enemy;

    public BatIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (stateMachine.lastState is BatAttackState)
            stateTimer = enemy.attackCooldown;
        else
            stateTimer = 0.4f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            if(enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 1.6f * enemy.facingDir,enemy.transform.position.y + -0.45f), 0.7f) && !(enemy.AttackType == 1 && stateMachine.lastState is BatAttackState))
            {
                enemy.AttackType = 1;
                stateMachine.ChangeState(enemy.attackState);
            }
            else if(enemy.CanAttackPlayer(enemy.transform.position, 8f) && enemy.transform.position.y > enemy.MinHeight && Random.Range(0,100) > 0 && enemy.AttackType != 0)
            {
                enemy.AttackType = 0;
                stateMachine.ChangeState(enemy.attackState);
            }
            else if(enemy.transform.position.y > enemy.MinHeight && enemy.CanAttackPlayer(enemy.transform.position, 6.5f))
            {
                enemy.AttackType = 2;
                stateMachine.ChangeState(enemy.attackState);
            }  
            else
                stateMachine.ChangeState(enemy.flyState);
        }   
    }
}
