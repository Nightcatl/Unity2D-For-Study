using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject ui;
    public static UI instance;

    public GameObject[] menu;

    public GameObject UI_InGame;

    [Header("All UI Menu")]
    public UI_Characte characte;
    public UI_Craft craft;
    public UI_Items items;
    public UI_Option option;
    public UI_Skill skill;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        characte.Initialize();
        craft.Initialize();
        items.Initialize();
        option.Initialize();
        skill.Initialize();
    }

    protected virtual void Start()
    {

    }

    void Update()
    {

    }

    public virtual void SwithTo(GameObject _menu)
    {
        foreach(GameObject oldMenu in menu)
        {
            oldMenu.SetActive(false);
        }

        if(_menu != null)
            _menu.SetActive(true);
    }
}
