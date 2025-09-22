using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UI_SkillSlot skill;

    [SerializeField] private UI_SkillTreeSlot lastSkillTreeSlot;
    [SerializeField] private SkillLevelType skillType;
    public Image skillImage;
    public bool IsUnlock = false;

    private void Start()
    { 
        if(skillType == SkillLevelType.Start)
            IsUnlock = true;

        if(!IsUnlock)
            skillImage.color = Color.gray;
        else
            skillImage.color = Color.white;
    }

    public void SetUpSkillTreeSlot(UI_SkillSlot _skill)
    {
        skill = _skill;
        skillImage.sprite = _skill.skillImage.sprite;

        UpdateSlot();
    }

    public void UnlockSkill()
    { 
        if (IsUnlock)
        {
            Debug.Log("Unlock");
            return;
        }

        if(skill.levelType != SkillLevelType.Start)
        {
            Debug.Log("error");
            return;
        }

        Debug.Log("Success");
        skill.levelType = skillType;
        IsUnlock = true;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (skill.levelType == skillType || skillType == SkillLevelType.Start)
            IsUnlock = true;
        else
            IsUnlock = false;

        if (!IsUnlock)
            skillImage.color = Color.gray;
        else
            skillImage.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnlockSkill();
        skill.SetUpSkillLevel();
    }
}
