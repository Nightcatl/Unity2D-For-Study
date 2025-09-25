using UnityEngine;

public class Fire_Skill : Skill
{
    public static Fire_Skill instance;

    private GameObject clone;
    private AttributeType magicType = AttributeType.Fire;

    private int fireball_maxnum;
    private int fireball_num;

    private GameObject fireBall;
    private GameObject flameRays;

    [Header("Fire Ball")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireball_cooldown;
    [SerializeField] private float fireball_Damage;
    [SerializeField] private float fireball_Probability;
    [SerializeField] private float fireball_Poisedamage;
    [SerializeField] private Vector2 fireball_KnockPower;
    [SerializeField] private float fireball_Duration;
    [SerializeField] private SkillLevelType fireballType;
    [SerializeField] private float fireball_ExitTime;
    [SerializeField] private float moveSpeed;
    public SkillData FireBall;

    [Header("Flame Rays")]
    [SerializeField] private GameObject flameRaysPrefab;
    [SerializeField] private float flameRays_cooldown;
    [SerializeField] private float flameRays_Damage;
    [SerializeField] private float flameRays_Probability;
    [SerializeField] private float flameRays_Poisedamage;
    [SerializeField] private Vector2 flameRays_KnockPower;
    [SerializeField] private float flameRays_duration;
    [SerializeField] private SkillLevelType flameRaysType;
    

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private Vector3 offset;

    private float fireball_cooldownTimer;
    private float flameRays_cooldownTimer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    protected override void Start()
    {
        base.Start();

        if (fireballType == SkillLevelType.Left)
            fireball_maxnum = 3;
        else
            fireball_maxnum = 1;
    }

    protected override void Update()
    {
        base.Update();

        fireball_cooldownTimer -= Time.deltaTime;
        flameRays_cooldownTimer -= Time.deltaTime;

        if(fireball_cooldownTimer < 0 && fireball_num < fireball_maxnum)
        {
            fireball_num = fireball_maxnum;
        }

    }

    public override void UseSkill()
    {
        base.UseSkill();

        switch (player.magicType.magicNum)
        {
            case 0:
                if(fireball_cooldownTimer <= 0 || fireball_num > 0)
                {
                    UseFireball();
                    fireball_cooldownTimer = fireball_cooldown;
                }else
                {
                    player.IsOver = true;
                    Debug.Log("skill is cooldown");
                }
                break;
            case 1:
                if(flameRays_cooldownTimer <= 0)
                {
                    UseFlamesRays();
                    flameRays_cooldownTimer = flameRays_cooldown;
                }else
                {
                    player.IsOver = true;
                    Debug.Log("skill is cooldown");
                }
                
                break;
        }
    }

    public void UseFireball()
    {
        if(!player.IsAim)
        {
            AimSquare.SetActive(true);
            player.IsAim = true;

            Time.timeScale = 0.1f;

            return;
        }

        player.stat.currentMagicpoint -= FireBall.magicPointReduce;

        Time.timeScale = 1;

        StartCoroutine(Recovery(fireball_Duration));

        player.IsAim = false;

        AimSquare.SetActive(false);

        fireball_num--;

        fireBall = Instantiate(fireballPrefab, AimSquare.transform.position, Quaternion.identity);
        fireBall.transform.Rotate(0, 0, AimSquare_Controller.instance.Z);

        Fireball_Skill_Controller fireball_Skill_Controller = fireBall.GetComponent<Fireball_Skill_Controller>();

        fireball_Skill_Controller.SetupSkill(fireball_Damage, fireball_Probability, fireball_Poisedamage, fireball_KnockPower, false, moveSpeed, player, magicType, fireballType);
        fireball_Skill_Controller.SetUpExitTime(fireball_ExitTime);

        StartCoroutine(Recovery(0));
    }

    public void UseFlamesRays()
    {
        if(flameRaysType == SkillLevelType.Left)
        {
            clone = Instantiate(clonePrefab, player.transform.position + offset, Quaternion.identity);

            Clone_Skill_Controller clone_Skill_Controller = clone.GetComponent<Clone_Skill_Controller>();

            clone_Skill_Controller.setUpClone(flameRays_Damage, flameRays_Probability, flameRays_Poisedamage, flameRays_KnockPower, player, magicType, flameRays_duration, player.facingDir, flameRaysType);

            player.IsOver = true;

            return;
        }

        flameRays = Instantiate(flameRaysPrefab, Magic.transform.position, Quaternion.identity);

        FlameRays_Skill_Controller fireball_Skill_Controller = flameRays.GetComponent<FlameRays_Skill_Controller>();

        fireball_Skill_Controller.SetupSkill(flameRays_Damage, flameRays_Probability, flameRays_Poisedamage, flameRays_KnockPower, true, 0, player, magicType, flameRaysType);
    }

    public override void SetSkillLevel(int magicNum, SkillLevelType _skillType)
    {
        switch(magicNum)
        {
            case 0:
                fireballType = _skillType;
                break;
            case 1:
                flameRaysType = _skillType;
                break;
        }
    }

    public override SkillLevelType ReturnSkillLevel(int magicNum)
    {
        switch (magicNum)
        {
            case 0:
                return fireballType;
            case 1:
                return flameRaysType;
        }

        return 0;
    }
}
