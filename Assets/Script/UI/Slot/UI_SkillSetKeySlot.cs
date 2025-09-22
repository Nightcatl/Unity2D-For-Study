using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSetKeySlot : MonoBehaviour, IDropHandler
{
    public KeyCode skillKey;
    public SkillType magicType;
    public Image skillImage;

    public UI_SkillKeySlot skillKeySlot;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        UI_SkillSlot ui_SkillSlot = dropped.GetComponent<UI_SkillSlot>();

        if(ui_SkillSlot != null )
        {
            skillImage.sprite = ui_SkillSlot.skillImage.sprite;
            magicType = ui_SkillSlot.skillType;
            SetSkillKey();
        }
    }

    public void SetSkillKey()
    {
        skillKeySlot.magicType = magicType;
        skillKeySlot.skillImage.sprite = skillImage.sprite;
    }
}
