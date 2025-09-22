using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Space]
    public GameObject thunderPrefab;

    #region States
    public BatHideState hideState;
    public BatIdleState idleState;
    public BatFlyState flyState;
    public BatAttackState attackState;
    public BatDeadState deadState;
    public BatDeathState deathState;
    #endregion

    [Header("Fly Info")]
    public float MaxHeight;
    public float MinHeight;

    [Header("Magic Info")]
    public float magicdamage;
    public float probability;
    public float posisedamage;
    public Vector2 knockPower;
    public bool CanContinuous;
    public float Skill_moveSpeed;

    public int AttackType;

    protected override void Awake()
    {
        base.Awake();

        hideState = new BatHideState(this, stateMachine, "Idle", this);
        idleState = new BatIdleState(this, stateMachine, "Fly", this);
        flyState = new BatFlyState(this, stateMachine, "Fly", this);
        attackState = new BatAttackState(this, stateMachine, "Attack",this);
        deadState = new BatDeadState(this, stateMachine, "Dead", this);
        deathState = new BatDeathState(this, stateMachine, "Death", this);
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

    public GameObject UseSkill(Vector2 targetPosition)
    {
        GameObject thunder = Instantiate(thunderPrefab, new Vector2(targetPosition.x, targetPosition.y + 1.8f), Quaternion.identity);

        return thunder;
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
