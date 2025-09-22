using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash_Skill_Controller : Skill_Controller
{
    public override void SetupSkill(float _magicdamage, float _probability, float _poisedamage, Vector2 _knockPower, bool _CanContinuous, float _moveSpeed, Player _player, AttributeType _musicType, SkillLevelType _skillType)
    {
        base.SetupSkill(_magicdamage, _probability, _poisedamage, _knockPower, _CanContinuous, _moveSpeed, _player, _musicType, _skillType);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SelfDestroy()
    {
        base.SelfDestroy();

        player.IsOver = true;
    }
}
