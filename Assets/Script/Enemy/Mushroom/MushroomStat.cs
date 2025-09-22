using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomStat : EnemyStat
{
    private Enemy_Mushroom enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy_Mushroom>();
    }

    public override void TakeDamage(float _damage, Vector2 knockPower, float _poisedamage, int hitDir)
    {
        base.TakeDamage(_damage, knockPower, _poisedamage, hitDir);
    }

    public override void TakeMagicDamage(float _magicdamage, Vector2 knockPower, float _poisedamage, float _probability, AttributeType _debuffType)
    {
        base.TakeMagicDamage(_magicdamage, knockPower, _poisedamage, _probability, _debuffType);
    }
}
