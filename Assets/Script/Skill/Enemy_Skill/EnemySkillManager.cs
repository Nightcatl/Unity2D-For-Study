using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public static EnemySkillManager instance;

    [Header("SkillData")]
    public SkillData bat_Skill;
    public SkillData redHood_Arrow;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
