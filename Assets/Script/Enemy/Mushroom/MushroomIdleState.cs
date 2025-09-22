using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomIdleState : EnemyState
{
    Enemy_Mushroom enemy;
    Player player;

    public MushroomIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (enemy.SeekPlayer)
            stateTimer = enemy.idleTime;
        else if (stateMachine.lastState == enemy.attackState)
            stateTimer = enemy.battleDuration + Random.Range(-0.1f, 0.2f);

        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsBreak)
            return;

        if(!enemy.CheckPlayerFacing())
            enemy.Flip();

        if (stateTimer <= 0 && !enemy.SeekPlayer && enemy.IsGroundDetected())
        {
            if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 1.66f * enemy.facingDir, enemy.transform.position.y + -0.76f), 0.9f))
                stateMachine.ChangeState(enemy.attackState);
            else if (!enemy.IsWallDetected())
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }

        if (Vector2.Distance(player.transform.position, enemy.transform.position) >= 20)
        {
            if (!enemy.SeekPlayer)
            {
                enemy.SeekPlayer = true;

                enemy.SeekTimer = enemy.seekTime;
            }
        }

        if (enemy.SeekPlayer)
        {
            if (enemy.SeekTimer <= 0)
            {
                stateMachine.ChangeState(enemy.hidedState);
            }
            else if (stateTimer <= 0)
            {
                if (enemy.IsGroundDetected() && !enemy.IsWallDetected())
                    stateMachine.ChangeState(enemy.moveState);
            }
        }
    }
}
