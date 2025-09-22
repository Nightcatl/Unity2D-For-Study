using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder_Skill : Skill
{
    public static Thunder_Skill instance;

    private AttributeType magicType = AttributeType.Thunder;

    private GameObject thunderSplash;
    private GameObject lightning;

    private int thunderSplash_maxnum;
    private int thunderSplash_num;

    [Header("Thunder Splash")]
    [SerializeField] private GameObject thunderSplashPrefab;
    [SerializeField] private float thunserSplash_cooldown;
    [SerializeField] private float thunderSplash_Damage;
    [SerializeField] private float thunderSplash_Probability;
    [SerializeField] private float thunderSplash_Poisedamage;
    [SerializeField] private Vector2 thunderSplash_KnockPower;
    [SerializeField] private float thunderSplash_duration;
    [SerializeField] private SkillLevelType thunderSplashType;

    [Header("Lightning")]
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private float lightning_Damage;
    [SerializeField] private float lightning_Probability;
    [SerializeField] private float lightning_Poisedamage;
    [SerializeField] private Vector2 lightning_KnockPower;
    [SerializeField] private float lightning_duration;
    [SerializeField] private SkillLevelType lightningType;
    [SerializeField] private float lightning_moveSpeed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    protected override void Start()
    {
        base.Start();

        if (thunderSplashType == SkillLevelType.Right)
            thunderSplash_maxnum = 3;
        else
            thunderSplash_maxnum = 1;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        switch (player.magicType.magicNum)
        {
            case 1:
                UseThunderSplash();
                break;
            case 2:
                UseLightning();
                break;
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    private void UseThunderSplash()
    {
        thunderSplash_num = thunderSplash_maxnum;
        
        if (thunderSplashType == SkillLevelType.Left)
        {
            StartCoroutine(Recovery(thunderSplash_duration));

            GameObject thunderSplashPrefabPro = thunderSplashPrefab;

            thunderSplashPrefabPro.transform.localScale = new Vector3(1.5f, 1.5f, 1);

            thunderSplash = Instantiate(thunderSplashPrefab,
                new Vector3(player.transform.position.x + 7 * player.facingDir, player.transform.position.y + 2.2f, 0), Quaternion.identity);

            ThunderSplash_Skill_Controller thunderSplash_Skill_Controller = thunderSplash.GetComponent<ThunderSplash_Skill_Controller>();

            thunderSplash_Skill_Controller.SetupSkill(thunderSplash_Damage, thunderSplash_Probability, thunderSplash_Poisedamage, thunderSplash_KnockPower, true, 0, player, magicType, thunderSplashType);
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(12, 4), 0);

            if (colliders.Length > 0)
            {
                StartCoroutine(Recovery(thunderSplash_duration));

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<EnemyStat>() != null)
                    {

                        thunderSplash = Instantiate(thunderSplashPrefab, hit.transform.position, Quaternion.identity);

                        ThunderSplash_Skill_Controller thunderSplash_Skill_Controller = thunderSplash.GetComponent<ThunderSplash_Skill_Controller>();

                        thunderSplash_Skill_Controller.SetupSkill(thunderSplash_Damage, thunderSplash_Probability, thunderSplash_Poisedamage, thunderSplash_KnockPower, false, 0, player, magicType, thunderSplashType);

                        thunderSplash_num--;
                    }
                    if (thunderSplash_num <= 0)
                        return;
                }
            }
            else
            {
                StartCoroutine(Recovery(0));

                Debug.Log("NO TARGET");
            }
        }

       
    }

    private void UseLightning()
    {
        StartCoroutine(Recovery(lightning_duration));

        lightning = Instantiate(lightningPrefab, Magic.transform.position, Quaternion.identity);

        Lightning_Skill_Controller lightning_Skill_Controller = lightning.GetComponent<Lightning_Skill_Controller>();

        lightning_Skill_Controller.SetupSkill(lightning_Damage, lightning_Probability, lightning_Poisedamage, lightning_KnockPower, false,lightning_moveSpeed, player, magicType, lightningType);
        lightning_Skill_Controller.SetUpSplit(100);
    }

    public override void SetSkillLevel(int magicNum, SkillLevelType _skillType)
    {
        switch (magicNum)
        {
            case 0:
                thunderSplashType = _skillType;
                break;
            case 1:
                lightningType = _skillType;
                break;
        }
    }

    public override SkillLevelType ReturnSkillLevel(int magicNum)
    {
        switch (magicNum)
        {
            case 0:
                return thunderSplashType;
            case 1:
                return lightningType;
        }

        return 0;
    }
}
