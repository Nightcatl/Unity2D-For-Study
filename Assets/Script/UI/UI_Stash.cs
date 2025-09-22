using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Stash : UI_ItemSlot
{
    [SerializeField] private TextMeshProUGUI itemText_Name;
    private string itemName;

    public override void UpdataSlotUI()
    {
        base.UpdataSlotUI();

        itemName = item.Data.name;

        itemText_Name.text = itemName;
    }
}
