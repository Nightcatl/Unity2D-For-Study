using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MaterialSlot : UI_ItemSlot
{
    public InventoryItem selectItem;

    protected override void Start()
    {
        base.Start();

        selectItem = null;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        
        ItemData_Material material = item.Data as ItemData_Material;

        if (material.IsExample)
        {
            UI_Craft.instance.AddMaterialSelectSlot(material,this);
        }
    }
}
