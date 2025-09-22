using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goblin : Enemy
{
    #region State
    public GoblinHideState hideState;
    public GoblinIdleState idleState;
    public GoblinMoveState moveState;
    public GoblinAttackState attackState;
    public GoblinDeadState deadState;
    public GoblinDeathState deathState;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        hideState = new GoblinHideState(this, stateMachine, "Hide", this);
        idleState = new GoblinIdleState(this, stateMachine, "Idle", this);
        moveState = new GoblinMoveState(this, stateMachine, "Move", this);
        attackState = new GoblinAttackState(this, stateMachine, "Attack", this);
        deadState = new GoblinDeadState(this, stateMachine, "Dead", this);
        deathState = new GoblinDeathState(this, stateMachine, "Death", this);
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
