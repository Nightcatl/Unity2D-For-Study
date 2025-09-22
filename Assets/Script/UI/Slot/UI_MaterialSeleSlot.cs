using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MaterialSeleSlot : UI_ItemSlot
{
    public UI_MaterialSlot slot;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item != null)
        {
            slot.selectItem = item;

            slot.SetImage(itemImage);
            UI_Craft.instance.RemoveMaterialSelectSlot();
        }
    }
}
