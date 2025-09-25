using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Info", menuName = "Skill/Info")]
public class SkillData : ScriptableObject
{
    public GameObject SkillPrefab;

    public string SkillName;

    public AttributeType attributeType;
    public SkillLevelType skillLevelType;

    public float Damage;
    public float Probability;
    public float Poisedamage;
    public Vector2 KnockPower;

    public float Cooldown;
    public float RecoveryTime;
    public float ExitTime;
    public float MoveSpeed;

    public float magicPointReduce;
}
