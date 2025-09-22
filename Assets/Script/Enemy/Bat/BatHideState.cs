using UnityEngine;

public class BatHideState : EnemyState
{
    public Enemy_Bat enemy;

    public BatHideState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Bat _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.FindPlayer)
        {
            enemy.AttackType = 2;
            stateMachine.ChangeState(enemy.attackState);
        }
    }
}
