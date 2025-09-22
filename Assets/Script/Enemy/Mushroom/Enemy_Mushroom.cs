using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    #region States
    public MushroomHideState hideState;
    public MushroomAwakeState awakeState;
    public MushroomIdleState idleState;
    public MushroomMoveState moveState;
    public MushroomAttackState attackState;
    public MushroomHidedState hidedState;
    public MushroomBreakState breakState;
    public MushroomStunnedState stunnedState;
    public MushroomDeadState deadState;
    public MushroomDeathState deathState;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        hideState = new MushroomHideState(this, stateMachine, "Hide", this);
        awakeState = new MushroomAwakeState(this, stateMachine, "Awake", this);
        idleState = new MushroomIdleState(this, stateMachine, "Idle", this);
        moveState = new MushroomMoveState(this, stateMachine, "Move", this);
        attackState = new MushroomAttackState(this, stateMachine, "Attack", this);
        hidedState = new MushroomHidedState(this, stateMachine, "Hided", this);
        breakState = new MushroomBreakState(this, stateMachine, "Break", this);
        stunnedState = new MushroomStunnedState(this, stateMachine, "Stunned", this);
        deadState = new MushroomDeadState(this, stateMachine, "Dead", this);
        deathState = new MushroomDeathState(this, stateMachine, "Death", this);
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

    protected override void Break()
    {
        base.Break();

        if (stat.currentTenacity > 0)
            return;

        if (facingDir == hitDir)
            Flip();

        stateMachine.ChangeState(breakState);
    }
}
