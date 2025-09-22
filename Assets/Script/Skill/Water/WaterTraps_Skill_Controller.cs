using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTraps_Skill_Controller : Skill_Controller
{
    private Rigidbody2D rb;

    private float ExitTime;

    public bool isHit;

    public override void SetupSkill(float _magicdamage, float _probability, float _poisedamage, Vector2 _knockPower, bool _CanContinuous, float _moveSpeed, Player _player, AttributeType _musicType, SkillLevelType _skillType)
    {
        base.SetupSkill(_magicdamage, _probability, _poisedamage, _knockPower, _CanContinuous, _moveSpeed, _player, _musicType, _skillType);
    }

    public void SetUpExitTime(float _ExitTime)
    {
        ExitTime = _ExitTime;
    }

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

    }

    protected override void Update()
    {
        base.Update();

        ExitTime -= Time.deltaTime;

        if(ExitTime < 0)
            SelfDestroy();
    }
}
