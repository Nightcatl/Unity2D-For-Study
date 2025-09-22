using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillKeySlot : MonoBehaviour,IPointerDownHandler
{
    public KeyCode skillKey;
    public SkillType magicType;
    public Image skillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        UseSkill();
    }

    public void UseSkill()
    {
        PlayerManager.instance.player.useSkill = true;
        PlayerManager.instance.player.magicType = magicType;

        Time.timeScale = 1;

        PlayerManager.instance.player.Roulette.SetActive(false);
    }
}
