using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour,IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UI_Skill skill;
    public Vector3 lastPosition;
    public Image skillImage;
    [SerializeField] private TextMeshProUGUI SkillName_Text;
    [SerializeField] private GameObject SkillTree;

    public Transform parentAfterDrag;
    
    private UI_SkillTreeSlot[] skillTreeSlot;

    public SkillType skillType;
    public string skillName;
    public SkillLevelType levelType;

    private void Awake()
    {
        skill = GetComponentInParent<UI_Skill>();
    }

    private void Start()
    {
        SkillName_Text.text = skillName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!skill.CanDrop)
        {
            skillTreeSlot = SkillTree.GetComponentsInChildren<UI_SkillTreeSlot>();
            foreach (var slot in skillTreeSlot)
            {
                slot.SetUpSkillTreeSlot(this);
            }
        }
    }

    #region Drag
    public void SetUpSkillLevel()
    {
        switch(skillType.attributeType)
        {
            case AttributeType.Fire:
                SkillManager.instance.fire.SetSkillLevel(skillType.magicNum, levelType);
                break;
            case AttributeType.Water:
                SkillManager.instance.water.SetSkillLevel(skillType.magicNum, levelType);
                break;
            case AttributeType.Thunder:
                SkillManager.instance.thunder.SetSkillLevel(skillType.magicNum, levelType);
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(skill.CanDrop)
        {
            lastPosition = skillImage.transform.position;
            parentAfterDrag = skillImage.transform.parent;
            skillImage.transform.SetParent(transform.root);
            skillImage.transform.SetAsLastSibling();
            skillImage.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    { 
        if(skill.CanDrop)
        {
            skillImage.transform.position = Input.mousePosition;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(skill.CanDrop)
        {
            skillImage.transform.position = lastPosition;
            skillImage.transform.SetParent(parentAfterDrag);
            skillImage.raycastTarget = true;
        }
    }
    #endregion
}
