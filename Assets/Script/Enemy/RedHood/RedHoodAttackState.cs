using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHoodAttackState : EnemyState
{
    private Enemy_RedHood enemy;
    private Player player;

    public RedHoodAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_RedHood _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        player = PlayerManager.instance.player;

        enemy.SetZeroVelocity();

        enemy.anim.SetInteger("AttackCount", enemy.attackCount);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttack = Time.time;

        if (enemy.attackCount >= 4)
            enemy.attackCount = 0;
        else
            enemy.attackCount++;
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            if (enemy.CanAttackPlayer(new Vector2(enemy.transform.position.x + 0, enemy.transform.position.y + -1f), 0.7f))
            {
                if (player.isBlock)
                    enemy.attackCount = 4;

                stateMachine.ChangeState(enemy.idleState);
            }
            else
                stateMachine.ChangeState(enemy.idleState);
                
        }
        
            
    }
}
