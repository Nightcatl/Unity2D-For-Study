using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    [SerializeField] private TextMeshProUGUI itemText_Name;
    private string itemName;

    public void UpdateCraftEquipmentUI()
    {
        UpdataSlotUI();

        itemName = item.Data.name;
        itemText_Name.text = itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        UI_Craft.instance.UpdateCraftMaterialUI(item);
    }
}
