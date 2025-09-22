using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TMPro.TextMeshProUGUI itemText_Amount;

    public EquipmentType slotType;
    public InventoryItem_Equipment item;

    public int slotIndex = 0;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public void UpdateEquipmentUI()
    {
        UpdataSlotUI();
    }

    public virtual void UpdataSlotUI()
    {
        if (item != null && item.Data != null)
        {
            itemImage.color = Color.white;

            itemImage.sprite = item.Data.itemIcon;

            if (item.StackSize >= 1)
            {
                itemText_Amount.text = item.StackSize.ToString();
            }
            else
            {
                itemText_Amount.text = "";
            }
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;

            itemText_Amount.text = "";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        Inventory.instance.UnEquipItem(item, item.StackSize , slotIndex);
    }
}
