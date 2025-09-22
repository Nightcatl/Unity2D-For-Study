using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_Skill : MonoBehaviour
{
    public GameObject[] menu;

    public bool CanDrop;

    public UI_SkillSlot[] SkillSLotList;

    public void Initialize()
    {
        SkillSLotList = GetComponentsInChildren<UI_SkillSlot>();
    }

    protected void Start()
    {
        SwithTo(menu[0]);
    }

    public void SwithTo(GameObject _menu)
    {
        foreach (GameObject oldMenu in menu)
        {
            oldMenu.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);

        if (menu[1] == _menu)
        {
            CanDrop = true;
        }
        else
        {
            CanDrop = false;
        }
    }
}
