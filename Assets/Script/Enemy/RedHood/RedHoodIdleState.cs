using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodIdleState : EnemyState
{
    private Enemy_RedHood enemy;
    private Player player;

    public RedHoodIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if ((stateMachine.lastState is RedHoodAttackState && enemy.attackCount == 0) || stateMachine.lastState is RedHoodArcheryState)
            stateTimer = enemy.idleTime;

        if(stateMachine.lastState is RedHoodIdleState)
            stateTimer = enemy.idleTime - 0.4f;

        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.CheckAttackCooldown() && stateTimer <= 0)
        {
            if (!enemy.CheckPlayerFacing())
                enemy.Flip();

            if (stateMachine.lastState is RedHoodDashState)
            {
                if (Random.Range(0, 100) > 70)
                {
                    enemy.attackCount = 0;

                    stateMachine.ChangeState(enemy.attackState);

                    return;
                }
                else
                {
                    enemy.attackCount = 3;

                    stateMachine.ChangeState(enemy.attackState);

                    return;
                }
            }

            if(Vector2.Distance(player.transform.position, enemy.transform.position) > 5)
            {
                if (stateMachine.lastState is RedHoodLandingState)
                {
                    stateMachine.ChangeState(enemy.archeryState);

                    return;
                }
                else if (Random.Range(0, 100) > 40 && enemy.CheckBorder())
                {
                    enemy.jumpDir = -1;

                    stateMachine.ChangeState(enemy.jumpState);

                    return;
                }
                else if (Random.Range(0, 100) > 30)
                {
                    stateMachine.ChangeState(enemy.archeryState);

                    return;
                }
                else
                {
                    stateMachine.ChangeState(enemy.moveState);

                    return;
                }  
            }

            if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 0, enemy.transform.position.y + -1f), 0.7f))
            {
                if (enemy.attackCount != 0)
                    stateMachine.ChangeState(enemy.attackState);
                else if (Random.Range(0, 100) > 70)
                {
                    stateMachine.ChangeState(enemy.dashState);
                }
                else
                    stateMachine.ChangeState(enemy.attackState);
            }
            else if (stateMachine.lastState is RedHoodIdleState)
                stateMachine.ChangeState(enemy.moveState);
            else
                stateMachine.ChangeState(enemy.idleState);
        }
    }
}
