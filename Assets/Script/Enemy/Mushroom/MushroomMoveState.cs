using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMoveState : EnemyState
{
    Enemy_Mushroom enemy;
    Player player;

    public MushroomMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (enemy.IsBreak)
            return;

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 1.66f * enemy.facingDir, enemy.transform.position.y + -0.76f), 0.9f))
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if (!enemy.CheckPlayerFacing())
            enemy.Flip();

        if (Vector2.Distance(PlayerManager.instance.player.transform.position, enemy.transform.position) >= 15 && stateTimer <= 0 && !enemy.SeekPlayer)
        {
            stateMachine.ChangeState(enemy.idleState);
            enemy.SeekPlayer = true;
        }

        if (enemy.SeekPlayer && stateTimer <= 0)
        {
            if (enemy.SeekTimer <= 0)
            {
                stateMachine.ChangeState(enemy.hidedState);
            }
            else if (stateTimer <= 0)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (!enemy.IsGroundDetected() && enemy.IsWallDetected())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
