using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Skill : Skill
{
    public static Water_Skill Instance;
    private AttributeType magicType = AttributeType.Water;

    private int waterTraps_maxnum;
    private int waterTraps_num;

    private GameObject waterSplash;
    private GameObject waterTraps;

    [Header("Water Traps")]
    [SerializeField] private GameObject waterTrapsPrefab;
    [SerializeField] private float waterTraps_cooldown;
    [SerializeField] private float waterTraps_Damage;
    [SerializeField] private float waterTraps_Probability;
    [SerializeField] private float waterTraps_Poisedamage;
    [SerializeField] private Vector2 waterTraps_KnockPower;
    [SerializeField] private float waterTraps_duration;
    [SerializeField] private SkillLevelType waterTrapsType;
    [SerializeField] private float waterTraps_ExitTime;

    [Header("Water Splash")]
    [SerializeField] private GameObject waterSplashPrefab;
    [SerializeField] private float waterSplash_cooldown;
    [SerializeField] private float waterSplash_Damage;
    [SerializeField] private float waterSplash_Probability;
    [SerializeField] private float waterSplash_Poisedamage;
    [SerializeField] private Vector2 waterSplash_KnockPower;
    [SerializeField] private float waterSplash_duration;
    [SerializeField] private SkillLevelType waterSplashType;

    private float waterTraps_cooldownTimer;
    private float waterSplash_cooldownTimer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(Instance);
    }

    protected override void Start()
    {
        base.Start();

        if (waterTrapsType == SkillLevelType.Left)
            waterTraps_maxnum = 3;
        else
            waterTraps_maxnum = 1;
    }

    protected override void Update()
    {
        base.Update();

        waterTraps_cooldownTimer -= Time.deltaTime;
        waterSplash_cooldownTimer -= Time.deltaTime;

        if(waterTraps_cooldownTimer < 0 && waterTraps_num < waterTraps_maxnum)
            waterTraps_num = waterTraps_maxnum;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        switch(player.magicType.magicNum)
        {
            case 1:
                if(waterTraps_cooldownTimer <= 0 || waterTraps_num > 0)
                {
                    UseWaterTraps();
                    waterTraps_num--;
                    waterTraps_cooldownTimer = waterTraps_cooldown;
                }
                else
                {
                    player.IsOver = true;
                    Debug.Log("skill is cooldown");
                }
                
                break;
            case 2:
                if(waterSplash_cooldownTimer <= 0)
                {
                    UseWaterSplash();
                    waterSplash_cooldownTimer = waterSplash_cooldown;
                }
                else
                {
                    player.IsOver = true;
                    Debug.Log("skill is cooldown");
                }
                
                break;
        }
    }

    private void UseWaterTraps()
    {
        StartCoroutine(Recovery(waterTraps_duration));

        waterTraps = Instantiate(waterTrapsPrefab, new Vector2(Magic.transform.position.x, player.transform.position.y), Quaternion.identity);

        WaterTraps_Skill_Controller waterTraps_Skill_Controller = waterTraps.GetComponent<WaterTraps_Skill_Controller>();

        waterTraps_Skill_Controller.SetupSkill(waterTraps_Damage, waterTraps_Probability, waterTraps_Poisedamage, waterTraps_KnockPower, true, 0, player, magicType, waterTrapsType);
        waterTraps_Skill_Controller.SetUpExitTime(waterTraps_ExitTime);
    }

    private void UseWaterSplash()
    {
        if(waterSplashType == SkillLevelType.Left)
        {
            for(int i = 0;i < 3; i++)
            {
                waterSplash = Instantiate(waterSplashPrefab, new Vector2(player.transform.position.x + (3f + i * 3) * player.facingDir , player.transform.position.y + 2.4f), Quaternion.identity);

                WaterSplash_Skill_Controller waterSplash_Skill_Controller = waterSplash.GetComponent<WaterSplash_Skill_Controller>();

                waterSplash_Skill_Controller.SetupSkill(waterSplash_Damage, waterSplash_Probability, waterSplash_Poisedamage, waterSplash_KnockPower, false, 0, player, magicType, waterSplashType);
            }

            StartCoroutine(Recovery(waterSplash_duration * .5f));
        }
        else
        {
            if(waterSplashType == SkillLevelType.Right)
                player.stateMachine.ChangeState(player.limitMoveState);

            waterSplash = Instantiate(waterSplashPrefab, new Vector2(player.transform.position.x  + 0.3f * player.facingDir, player.transform.position.y + 2.47f), Quaternion.identity);

            WaterSplash_Skill_Controller waterSplash_Skill_Controller = waterSplash.GetComponent<WaterSplash_Skill_Controller>();

            waterSplash_Skill_Controller.SetupSkill(waterSplash_Damage, waterSplash_Probability, waterSplash_Poisedamage, waterSplash_KnockPower, false, 0, player, magicType, waterSplashType);

            StartCoroutine(Recovery(waterSplash_duration));
        }
    }

    public void Retaliation()
    {
        waterSplash = Instantiate(waterSplashPrefab, new Vector2(player.enemy.position.x + 0.3f * player.facingDir, player.enemy.position.y + 2.47f), Quaternion.identity);

        WaterSplash_Skill_Controller waterSplash_Skill_Controller = waterSplash.GetComponent<WaterSplash_Skill_Controller>();

        waterSplash_Skill_Controller.SetupSkill(waterSplash_Damage, waterSplash_Probability, waterSplash_Poisedamage, waterSplash_KnockPower, true, 0, player, magicType, waterSplashType);
    }

    public override void SetSkillLevel(int magicNum, SkillLevelType _skillType)
    {
        switch (magicNum)
        {
            case 0:
                waterTrapsType = _skillType;
                break;
            case 1:
                waterSplashType = _skillType;
                break;
        }
    }

    public override SkillLevelType ReturnSkillLevel(int magicNum)
    {
        switch (magicNum)
        {
            case 0:
                return waterTrapsType;
            case 1:
                return waterSplashType;
        }

        return 0;
    }
}
