using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem_Equipment : InventoryItem
{
    public EquipmentItem equipment;

    public InventoryItem_Equipment(ItemData _newItemData,EquipmentItem _newEquipment) : base(_newItemData)
    {
        equipment = _newEquipment;
    }
}
