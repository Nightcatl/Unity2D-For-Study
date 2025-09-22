using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodMoveState : EnemyState
{
    private Enemy_RedHood enemy;
    private Player player;

    public RedHoodMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if(!enemy.CheckPlayerFacing())
            enemy.Flip();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        player = PlayerManager.instance.player;

        enemy.attackCount = 0;
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

        if (player.transform.position.y > enemy.transform.position.y)
            stateMachine.ChangeState(enemy.archeryState);

        if(enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 0, enemy.transform.position.y + -1f), 0.7f))
            stateMachine.ChangeState(enemy.idleState);
    }
}
