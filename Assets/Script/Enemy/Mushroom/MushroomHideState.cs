using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MushroomHideState : EnemyState
{
    protected Enemy_Mushroom enemy;

    public MushroomHideState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        enemy.sr.sortingLayerName = "Ground";
    }

    public override void Exit()
    {
        base.Exit();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.FindPlayer == true)
        {
            stateMachine.ChangeState(enemy.awakeState);
        }
    }
}
