using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string SlotName;
    [SerializeField] private StatName StatType;

    [SerializeField] private TextMeshProUGUI Text;

    private int statVaule;

    private void OnValidate()
    {
        gameObject.name = SlotName + " - Slot";
    }

    public void UpdateUI()
    {
        statVaule = PlayerManager.instance.player.stat.GetStatVaule(StatType);

        Text.text = SlotName + ":" + statVaule;
    }
}
