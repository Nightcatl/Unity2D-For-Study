using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region State
    public SkeletonIdleState idleState;
    public SkeletonMoveState moveState;
    public SkeletonAttackState attackState;
    public SkeletonBreakState takeHitState;
    public SkeletonHideState hideState;
    public SkeletonDeadState deadState;
    public SkeletonDeathState deathState;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        takeHitState = new SkeletonBreakState(this, stateMachine, "Break", this);
        hideState = new SkeletonHideState(this, stateMachine, "Hide", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Dead", this);
        deathState = new SkeletonDeathState(this, stateMachine, "Death", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(hideState);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Dead()
    {
        base.Dead();

        if (stat.currentHealth > 0)
            return;

        stateMachine.ChangeState(deadState);

        SeekPlayerPrefab.GetComponent<EnemySeekPlayer>().SeekPlayer();
    }
}
