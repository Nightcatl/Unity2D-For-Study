using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISaveManager
{
    [SerializeField] private UI_Skill ui_skill;

    public static SkillManager instance;

    public List<Skill_info> skillInfos;

    public Fire_Skill fire {  get; private set; }

    public Water_Skill water { get; private set; }

    public Thunder_Skill thunder { get; private set; }

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        fire = GetComponent<Fire_Skill>();
        water = GetComponent<Water_Skill>();
        thunder = GetComponent<Thunder_Skill>();

        foreach(var skill_info in skillInfos)
        {
            switch(skill_info.skillType.attributeType)
            {
                case AttributeType.Fire:
                    fire.SetSkillLevel(skill_info.skillType.magicNum, skill_info.levelType);
                    break;
                case AttributeType.Water:
                    water.SetSkillLevel(skill_info.skillType.magicNum, skill_info.levelType);
                    break;
                case AttributeType.Thunder:
                    thunder.SetSkillLevel(skill_info.skillType.magicNum, skill_info.levelType);
                    break;
            }
        }
    }

    public void UpdateSkillInfos()
    {
        for (int i = 0; i < skillInfos.Count; i++)
        {
            switch(skillInfos[i].skillType.attributeType)
            {
                case AttributeType.Fire:
                    skillInfos[i] = new Skill_info(skillInfos[i].skillType, fire.ReturnSkillLevel(skillInfos[i].skillType.magicNum));
                    break;
                case AttributeType.Water:
                    skillInfos[i] = new Skill_info(skillInfos[i].skillType, water.ReturnSkillLevel(skillInfos[i].skillType.magicNum));
                    break;
                case AttributeType.Thunder:
                    skillInfos[i] = new Skill_info(skillInfos[i].skillType, thunder.ReturnSkillLevel(skillInfos[i].skillType.magicNum));
                    break;
            }
        }
    }

    public void LoadData(GameData _data)
    {
        foreach (var skillToLoad in _data.skill)
        {
            for (int i = 0; i < skillInfos.Count; i++)
            {
                if (skillToLoad.skillType.attributeType == skillInfos[i].skillType.attributeType && skillToLoad.skillType.magicNum == skillInfos[i].skillType.magicNum)
                {
                        skillInfos[i] = skillToLoad;
                }
            }

            for(int i = 0;i < ui_skill.SkillSLotList.Length; i++)
            {
                if (skillToLoad.skillType.attributeType == ui_skill.SkillSLotList[i].skillType.attributeType && skillToLoad.skillType.magicNum == ui_skill.SkillSLotList[i].skillType.magicNum)
                {
                    ui_skill.SkillSLotList[i].levelType = skillToLoad.levelType;
                }

                /*switch (skillslot.skillType.attributeType)
                {
                    case AttributeType.Fire:
                        skillslot.levelType = fire.ReturnSkillLevel(skillslot.skillType.magicNum); 
                        break;
                    case AttributeType.Water:
                        skillslot.levelType = water.ReturnSkillLevel(skillslot.skillType.magicNum);
                        break;
                    case AttributeType.Thunder:
                        skillslot.levelType = thunder.ReturnSkillLevel(skillslot.skillType.magicNum);
                        break;
                }*/
            }
        }
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        _data.skill.Clear();

        UpdateSkillInfos();

        foreach(var skill_info in skillInfos)
        {
            _data.skill.Add(skill_info);
        }
    }
}
