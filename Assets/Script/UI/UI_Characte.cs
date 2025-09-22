using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Characte : MonoBehaviour
{
    [SerializeField] public UI_StatSlot[] statSlots;

    private void Awake()
    {
      statSlots = GetComponentsInChildren<UI_StatSlot>();  
    }

    public void Initialize()
    {
        statSlots = GetComponentsInChildren<UI_StatSlot>();
    }

    private void Start()
    {
        UpdateStatSlot();
    }

    public void UpdateStatSlot()
    {
        foreach (var slot in statSlots)
        {
            slot.UpdateUI();
        }
    }
}
