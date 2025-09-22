using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] protected TextMeshProUGUI itemText_Amount;
    [SerializeField] protected Image itemImage;

    public InventoryItem_Equipment item;
    private GameObject selectSlotParent = null;

    [SerializeField] private TextMeshProUGUI itemText_Name;
    private string itemName;

    public void UpdateInventoryUI()
    {
        UpdataSlotUI();

        if (item.equipment.IsCustomized)
            itemName = "Custimized + " + item.Data.name;
        else
            itemName = item.Data.name;

        itemText_Name.text = itemName;
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
        int slotIndex = 1;

        if(item != null)
        {
            if(item.equipment._Data.equipmentType == EquipmentType.Flask)
            {
                if (selectSlotParent != null)
                    return;

                selectSlotParent
                    = Instantiate(UI_Items.Instance.selectSlotParentPrefab, 
                    new Vector3(gameObject.transform.position.x - 116, gameObject.transform.position.y - 90, 0), Quaternion.identity, gameObject.transform);

                List<Button> selectSlotButton = new List<Button>();

                foreach(Button button in selectSlotParent.GetComponentsInChildren<Button>())
                    selectSlotButton.Add(button);

                foreach(Button button in selectSlotButton)
                {
                    button.onClick.AddListener(delegate(){
                        slotIndex =  selectSlotButton.IndexOf(button) + 1;
                        Inventory.instance.EquipItem(item.equipment, item.StackSize , slotIndex);
                        Destroy(selectSlotParent);
                    });
                }
            }
            else
            {
                Inventory.instance.EquipItem(item.equipment, slotIndex);
            }
        }
        
    }
}
