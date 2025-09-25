using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SkillLevelType
{
    Start,
    Left,
    Right
}

public enum AttributeType
{
    None,
    Fire,
    Water,
    Thunder,
    Physical,
}


[System.Serializable]
public struct SkillType
{
    public AttributeType attributeType;
    public int magicNum;

    public SkillType(AttributeType attributeType, int magicNum)
    {
        this.attributeType = attributeType;
        this.magicNum = magicNum;
    }

    public bool Equals(SkillType other)
    {
        return attributeType == other.attributeType && magicNum == other.magicNum;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(attributeType, magicNum);
    }
}

[System.Serializable]
public struct Skill_info
{
    public SkillType skillType;
    public SkillLevelType levelType;

    public Skill_info(SkillType _skillType, SkillLevelType _levelType)
    {
        skillType = _skillType;
        levelType = _levelType;
    }
}

public class Skill : MonoBehaviour
{
    protected Player player;
    [SerializeField] protected GameObject Magic;
    [SerializeField] protected GameObject AimSquare;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        CheckUnlock();
    }

    protected virtual void Update()
    {
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual void UseSkill()
    {

    }

    public virtual void SetSkillLevel(int magicNum, SkillLevelType _skillType)
    {
        
    }

    public virtual SkillLevelType ReturnSkillLevel(int magicNum)
    {
        return 0;
    }

    protected virtual IEnumerator Recovery(float _recoveryTime)
    {
        yield return new WaitForSeconds(_recoveryTime);

        player.IsOver = true;
    }
}
