using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttackState : EnemyState
{
    Enemy_Bat enemy;

    private Vector2 targetPosition;
    private Vector2 Speed;

    private float defaultSpeed;

    public BatAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);

        if (enemy.AttackType == 2)
        {
            stateTimer = 0.6f;

            targetPosition = PlayerManager.instance.player.transform.position;

            Vector2 findir = new Vector2(targetPosition.x - enemy.transform.position.x, targetPosition.y - enemy.transform.position.y);

            float Z = Mathf.Atan2(findir.normalized.y, findir.normalized.x);

            defaultSpeed = Vector2.Distance(targetPosition, enemy.transform.position) / stateTimer;

            Speed = new Vector2(defaultSpeed * Mathf.Cos(Z), defaultSpeed * Mathf.Sin(Z));

            enemy.anim.SetInteger("AttackType", enemy.AttackType);

            enemy.attackCheck.position = new Vector2(enemy.transform.position.x + 0.4f, enemy.transform.position.y);
            enemy.attackCheckRadius = 1.3f;
        }
        else if(enemy.AttackType == 1)
        {
            enemy.anim.SetInteger("AttackType", enemy.AttackType);

            enemy.attackCheck.position = new Vector2(enemy.transform.position.x + 1.6f,enemy.transform.position.y + -0.45f);
            enemy.attackCheckRadius = 0.7f;
        }else if(enemy.AttackType == 0)
        {
            stateTimer = 0.8f;

            enemy.anim.SetInteger("AttackType", enemy.AttackType);

            targetPosition = PlayerManager.instance.player.transform.position;

            GameObject thunder = enemy.UseSkill(targetPosition);

            Bat_Thuner bat_Thuner = thunder.GetComponent<Bat_Thuner>();
            bat_Thuner.SetupSkill(EnemySkillManager.instance.bat_Skill);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if(enemy.takeDamage)
            enemy.takeDamage = false;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.AttackType == 2)
        {
            enemy.SetVelocity(Speed.x, Speed.y);

            if (triggerCalled)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (enemy.AttackType == 0 && stateTimer <= 0)
            stateMachine.ChangeState(enemy.idleState);

        if(enemy.AttackType == 1)
        {
            if (triggerCalled)
                stateMachine.ChangeState(enemy.idleState);
        }

        if(enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.SetZeroVelocity();
            stateMachine.ChangeState(enemy.idleState);
        } 
    }
}
