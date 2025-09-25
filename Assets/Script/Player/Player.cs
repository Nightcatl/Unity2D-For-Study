using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity, ISaveManager
{
    [Space]
    [Header("Camera")]
    public CameraFollow cameraFollow;
    private float fallSpeedYDampingChangedThreshold;

    [Space]
    public GameObject Roulette;
    public UI_SkillKeySlot[] skillKeySlots;
    
    [HideInInspector] public Transform enemy;

    public Color[] changeColor;
    public Color defaultColor {  get;private set; }
    [HideInInspector] public int ColorIndex;

    [HideInInspector] public int particlesIndex;

    public float coyoteTime;
    [HideInInspector] public float lastInGoundTime;

    [Header("Jump info")]
    public float jumpForce;
    public float maxFallSpeed;
    [HideInInspector] public float defaultJumpForce;

    [Header("Attack info")]
    public Vector2[] attackMovement;
    [HideInInspector] public bool takeDamage;

    [Header("Block info")]
    public float blockTime;    
    public float dashSpeed;
    public float dashDuration;
    public Vector2 awaySpeed;
    public float awayDuration;
    [HideInInspector] public bool isBlock;
    [HideInInspector] public bool isBlockSuccess;

    [Header("Rolling Info")]
    public float rollingSpeed;
    public float rollingDuration;

    [HideInInspector] public SkillType magicType;
    [HideInInspector] public bool CanAirAttack;
    [HideInInspector] public bool IsAim;
    [HideInInspector] public bool IsOver;
    public bool IsTalk;
    [HideInInspector] public bool IsBusy;

    [Space]
    [HideInInspector] public CheckPoint NearCheckPoint;

    [HideInInspector] public bool ISChooseSkill;
    [HideInInspector] public bool useSkill = false;

    [HideInInspector] public List<Enemy> Body = new List<Enemy>();
    [HideInInspector] public List<Box> Box = new List<Box>();

    private int index_NPC = 0;
    [HideInInspector] public List<NPC> NpcCandidates = new List<NPC>();
    [HideInInspector] public NPC NPC;

    [SerializeField] private Sprite avator;

    [HideInInspector] public HashSet<Enemy> attckedEnemies;

    [SerializeField] private string filePath;

    private LayerMask enemyLayer;

    #region states

    public PlayerStateMachine stateMachine;
    public PlayerIdleState idleState {  get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerRollingState rollingState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerBlockState blockstate { get; private set; }
    public PlayerBlockSuccessState blockSuccessState { get; private set; }
    public PlayerCounterattackState counterattackState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerHitAndAwayState hitAndAwayState { get; private set; }
    public PlayerUsingMagicState usingMagicState { get; private set; }
    public PlayerLimitMoveState limitMoveState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        enemyLayer = PlayerManager.instance.enemy;

        stateMachine = new PlayerStateMachine();
        attckedEnemies = new HashSet<Enemy>();
    }

    protected override void Start()
    {
        base.Start();

        AssignStates();
        OnKnocked += ReturnToIdle;

        fallSpeedYDampingChangedThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        stateMachine.Initialize(idleState);

        cameraFollow = CameraFollow.instance;

        coyoteTime = 0.1f;

        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
        defaultColor = new Color(0.51f, 0.49f, 0.49f, 0);

        cameraFollow = CameraFollow.instance;
    }

    public void Initialized()
    {
        if (stateMachine == null)
            stateMachine = new PlayerStateMachine();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;

        enemy = null;

        CanAirAttack = true;

        stateMachine.Initialize(idleState);

        cameraFollow = CameraFollow.instance;

        fallSpeedYDampingChangedThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
    }

    protected override void Update()
    {
        base.Update();

        if (UseUI())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && IsTalk)
        {
            if (!UI_Dialog.instance.ContinueDialog())
            {
                return;
            }
        }

        if (stat.currentMagicpoint < stat.Magicpoint.GetValue())
            stat.currentMagicpoint += 0.3f * Time.deltaTime;

        if (IsBusy || LevelManager.instance.busy)
            return;

        CameraControll();

        if(takeDamage)
            AttackTrigger();

        if (isKnockback)
            return;

        ChooseSkill();
        CheckInteraction();
        UseCheckPoint();


        if (rb.velocity.y <= maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);

        stateMachine.currentState.Update();
    }

    private void CameraControll()
    {
        if (rb.velocity.y < fallSpeedYDampingChangedThreshold)
        {
            if (!CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling_Damp)
                CameraManager.instance.LerpYDamping(true);

            if (rb.velocity.y <= maxFallSpeed && !CameraManager.instance.IsLerpingYOffset && !CameraManager.instance.LerpedFromPlayerFalling_Offset)
                CameraManager.instance.LerpYOffest(true);
        }

        if (rb.velocity.y >= 0f)
        {
            if (!CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling_Damp)
            {
                CameraManager.instance.LerpedFromPlayerFalling_Damp = false;

                CameraManager.instance.LerpYDamping(false);
            }

            if (!CameraManager.instance.IsLerpingYOffset && CameraManager.instance.LerpedFromPlayerFalling_Offset)
            {
                CameraManager.instance.LerpedFromPlayerFalling_Offset = false;

                CameraManager.instance.LerpYOffest(false);
            }
        }
    }

    private void AssignStates()
    {
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Air");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        attackState = new PlayerAttackState(this, stateMachine, "Attack");
        rollingState = new PlayerRollingState(this, stateMachine, "Rolling");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        blockstate = new PlayerBlockState(this, stateMachine, "Block");
        blockSuccessState = new PlayerBlockSuccessState(this, stateMachine, "SuccessBlock");
        counterattackState = new PlayerCounterattackState(this, stateMachine, "Counterattack");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        hitAndAwayState = new PlayerHitAndAwayState(this, stateMachine, "HitAndAway");
        usingMagicState = new PlayerUsingMagicState(this, stateMachine, "UseMagic");
        limitMoveState = new PlayerLimitMoveState(this, stateMachine, "Idle");
    }

    private void UseCheckPoint()
    {
        if (Input.GetKeyDown(KeyCode.E) && NearCheckPoint != null)
        {
            NearCheckPoint.UseCheckPoint();
        }
    }

    private void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Body.Count > 0)
            {
                Body[0].PickUp();
                return;
            }

            if (Box.Count > 0)
            {
                Box[0].OpenBox();
                return;
            }

            if(NPC != null)
            {
                UI_Dialog.instance.SetAvator(avator);
                NPC.Talk();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && NpcCandidates.Count > 1)
        {
            ChangeNPC(index_NPC+1);
        }
    }

    public void ChangeNPC(int index = -1)
    {
        if (NpcCandidates.Count <= 0)
            return;

        if (index < 0)
            index = index_NPC;
        else
            index_NPC = index;

        if(index >= NpcCandidates.Count)
        {
            index = 0;
            index_NPC = 0;
        }

        NPC.SetCanTalk(false);
        NPC = NpcCandidates[index];
        NPC.SetCanTalk(true);
    }

    private void ChooseSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 0;
            skillKeySlots = Roulette.GetComponentsInChildren<UI_SkillKeySlot>();
            ISChooseSkill = true;
            Roulette.SetActive(true);
            Roulette.transform.position = Input.mousePosition;

            Time.timeScale = 0.1f;

            return;
        }

        if (ISChooseSkill)
        {
            foreach (UI_SkillKeySlot skillKeySlot in skillKeySlots)
            {
                if (Input.GetKeyDown(skillKeySlot.skillKey))
                {
                    skillKeySlot.UseSkill();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Roulette.SetActive(false);
            ISChooseSkill = false;
            Time.timeScale = 1;
        }
    }

    public void ReturnToIdle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void AnimationTrriger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Flip()
    {
        base.Flip();

        cameraFollow.CallTurn();
    }

    private bool UseUI()
    {
        if(Input.GetKeyUp(KeyCode.I))
        {
            if(UI.instance.ui.activeSelf == false)
            {
                Time.timeScale = 0;

                IsBusy = true;

                UI.instance.ui.SetActive(true);

                return true;
            }
            else
            {
                Time.timeScale = 1f;

                IsBusy = false;

                UI.instance.ui.SetActive(false);

                Debug.Log(1);

                return false;
            }
        }
       
        return false;
    }

    public void StartAttack()
    {
        InvokeRepeating("AttackTrigger", 0.1f, 0.01f);

        attckedEnemies.Clear();
    }

    public void EndAttack()
    {
        if (IsInvoking("AttackTrigger"))
        {
            CancelInvoke("AttackTrigger");
        }

        attckedEnemies.Clear();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, enemyLayer);

        float Damage = stat.Damage.GetValue() * (1 + stat.Strength.GetValue() * .15f);

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null && !enemy.IsDead && !attckedEnemies.Contains(enemy))
            {
                int hitDir = facingDir;

                enemy.stat.TakeDamage(Damage, konckbackPower[attackState.comboCounter], stat.Poisedamage.GetValue(), hitDir);

                attckedEnemies.Add(enemy);
            }
        }
    }

    #region Save And Load
    public void LoadData(GameData _data)
    {
        for (int i = 0; i < 16; i++)
        {
            StatName index = (StatName)i;

            if(_data.playerStat.TryGetValue(index, out var value))
                stat.SetStatVaule(index, value);
        }
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        _data.playerStat.Clear();

        for(int i = 0; i < 16; i++)
        {
            StatName index = (StatName)i;

            _data.playerStat.Add(index, stat.GetStatVaule(index));
        }
    }

    #endregion
}
