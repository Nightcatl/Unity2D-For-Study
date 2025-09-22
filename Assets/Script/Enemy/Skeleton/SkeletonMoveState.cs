using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    Enemy_Skeleton enemy;

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.SeekPlayer)
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

        if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 1.78f * enemy.facingDir, enemy.transform.position.y + -0.43f), 1f) 
            && (enemy.IsGroundDetected() || enemy.IsWallDetected()))
        {
            stateMachine.ChangeState(enemy.idleState);
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
