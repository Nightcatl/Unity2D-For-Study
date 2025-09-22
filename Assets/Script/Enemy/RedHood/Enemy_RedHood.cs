using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RedHood : Enemy
{
    public BossTrigger bossManager;

    public GameObject arrowPrefab;

    public Transform Left;
    public Transform Right;

    [Header("Jump info")]
    public float jumpForce;
    public float jumpTime;
    [HideInInspector] public float defaultJumpForce;

    public int jumpDir;

    #region States
    public RedHoodIdleState idleState;
    public RedHoodMoveState moveState;
    public RedHoodAttackState attackState;
    public RedHoodJumpState jumpState;
    public RedHoodAirState airState;
    public RedHoodLandingState landingState;
    public RedHoodArcheryState archeryState;
    public RedHoodDashState dashState;
    public RedHoodStartState startState;
    public RedHoodDeadState deadState;
    #endregion

    public int attackCount;
    public int archeryType;


    protected override void Awake()
    {
        base.Awake();

        idleState = new RedHoodIdleState(this, stateMachine, "Idle", this);
        moveState = new RedHoodMoveState(this, stateMachine, "Move", this);
        attackState = new RedHoodAttackState(this, stateMachine, "Attack", this);
        jumpState = new RedHoodJumpState(this, stateMachine, "Jump", this);
        airState = new RedHoodAirState(this, stateMachine, "Air", this);
        landingState = new RedHoodLandingState(this, stateMachine, "Landing", this);
        archeryState = new RedHoodArcheryState(this, stateMachine, "Archery", this);
        dashState = new RedHoodDashState(this, stateMachine, "Dash", this);
        startState = new RedHoodStartState(this, stateMachine, "Idle");
        deadState = new RedHoodDeadState(this, stateMachine, "Dead", this);

        stateMachine.Initialize(startState);
    }

    protected override void Start()
    {
        base.Start();

        facingRight = false;
        SetdefaultFacing(-1);
    }

    protected override void Update()
    {
        base.Update();

        if (LevelManager.instance.busy)
            return;
    }

    protected override void Dead()
    {
        base.Dead();

        if(stat.currentHealth <= 0)
        {
            Debug.Log("The boss is dead");
            stateMachine.ChangeState(deadState);
        }
    }

    public bool CheckBorder()
    {
        if (IsWallDetected())
            return true;
        else
            return false;
    }

    public override void StartToAttack()
    {
        base.StartToAttack();

        stateMachine.ChangeState(idleState);
    }
}
