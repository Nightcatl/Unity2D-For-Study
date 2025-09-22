using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMoveState : EnemyState
{
    private Enemy_Goblin enemy;

    public GoblinMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if(enemy.SeekPlayer)
            stateTimer = enemy.moveTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!enemy.CheckPlayerFacing())
            enemy.Flip();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 2f * enemy.facingDir, enemy.transform.position.y + -0.7f), 1.1f))
        {
            stateMachine.ChangeState(enemy.idleState);
            return;
        }

        if (Vector2.Distance(PlayerManager.instance.player.transform.position, enemy.transform.position) >= 20 && stateTimer <= 0 && !enemy.SeekPlayer)
        {
            stateMachine.ChangeState(enemy.idleState);
            enemy.SeekPlayer = true;

            enemy.SeekTimer = enemy.seekTime;
        }

        if (enemy.SeekPlayer && stateTimer <= 0)
        {
            if (enemy.SeekTimer <= 0)
            {
                stateMachine.ChangeState(enemy.hideState);
            }
            else if (stateTimer <= 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
