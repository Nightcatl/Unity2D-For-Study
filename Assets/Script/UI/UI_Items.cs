using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_Items : MonoBehaviour
{
    public static UI_Items Instance;

    public GameObject[] menu;

    public GameObject selectSlotParentPrefab;

    [SerializeField] private List<UI_InventorySlot> inventorySlots;
    private List<UI_ItemSlot> stashSlots;
    private UI_EquipmentSlot[] equipmentSlots;

    [SerializeField] private GameObject inventorySlotsPrefab;
    [SerializeField] private GameObject inventorySlotParent;

    [SerializeField] private GameObject stashSlotsPrefab;
    [SerializeField] private GameObject stashSlotParent;

    [SerializeField] private Transform equipmentParent;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;

        if (inventorySlots == null)
            inventorySlots = new List<UI_InventorySlot>();

        if (stashSlots == null)
            stashSlots = new List<UI_ItemSlot>();

        equipmentSlots = equipmentParent.GetComponentsInChildren<UI_EquipmentSlot>();

        SwithTo(menu[0]);
    }

    public void UpdateEquipmentUI()
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].UpdataSlotUI();
        }
    }

    public void UpdateInventoryUI()
    {
        foreach (UI_InventorySlot slot in inventorySlots)
        {
            slot.UpdateInventoryUI();
        }
    }

    public void UpdateStashUI(InventoryItem item = null)
    {
        foreach(UI_ItemSlot slot in stashSlots)
        {
            slot.UpdataSlotUI();
        }
    }

    public void AddEquipment(EquipmentItem newEquipment,InventoryItem_Equipment _equipment,int Index = 1)
    {
        for (int i = 0; i < 7; i++)
        {
            if (equipmentSlots[i].slotType == newEquipment._Data.equipmentType && Index == equipmentSlots[i].slotIndex)
            {
                equipmentSlots[i].item = _equipment;
                equipmentSlots[i].item.equipment.slotIndex = Index;
                equipmentSlots[i].UpdateEquipmentUI();
            }
        }
    }

    public void RemoveEquipment(EquipmentItem oldEquipment,int Index = 1)
    {
        for (int i = 0; i < 7; i++)
        {
            if (equipmentSlots[i].slotType == oldEquipment._Data.equipmentType && Index == equipmentSlots[i].slotIndex)
            {
                equipmentSlots[i].item.equipment.slotIndex = 0;
                equipmentSlots[i].item.equipment = null;
                equipmentSlots[i].item = null;
                equipmentSlots[i].UpdateEquipmentUI();
            }
        }
    }

    public void AddInventorySlot(EquipmentItem equipment, InventoryItem_Equipment inventoryItem)
    {
        if (equipment == null)
            return;

        GameObject inventorySlot =  Instantiate(inventorySlotsPrefab, inventorySlotParent.transform);

        UI_InventorySlot ui_InventorySlot = inventorySlot.GetComponent<UI_InventorySlot>();

        inventorySlots.Add(ui_InventorySlot);

        inventorySlots[inventorySlots.Count - 1].item.StackSize = inventoryItem.StackSize;
        inventorySlots[inventorySlots.Count - 1].item = inventoryItem;

        UpdateInventoryUI();
    }

    public void RemoveInventorySlot(EquipmentItem equipment)
    {
        if (equipment == null)
            return;

        for (int i = 0;i < inventorySlots.Count;i++)
        {
            if(inventorySlots[i].item.equipment == equipment)
            {
                UI_InventorySlot slot = inventorySlots[i];
                inventorySlots.Remove(slot);
                Destroy(slot.gameObject);
            }
        }
    }

    public void IncreseInventory(EquipmentItem equipment,int stack = 1)
    {
        foreach(UI_InventorySlot slot in inventorySlots)
        {
            if (slot.item.equipment == equipment)
            {
                //slot.inventory_Equipment.stackSize += stack;
                slot.UpdateInventoryUI();
            }
        }
    }

    public void AddStashSlot(InventoryItem item, int stack)
    {
        if (item == null)
            return;

        GameObject stashSlot = Instantiate(stashSlotsPrefab, stashSlotParent.transform);

        UI_ItemSlot ui_ItemSlot = stashSlot.GetComponent<UI_ItemSlot>();

        stashSlots.Add(ui_ItemSlot);

        stashSlots[stashSlots.Count - 1].item = item;

        UpdateStashUI();
    }

    public void RemoveStashSlot(InventoryItem item)
    {
        if (stashSlots == null)
            return;

        foreach(UI_ItemSlot slot in stashSlots)
        {
            if (slot.item == item)
            {
                stashSlots.Remove(slot);
                Destroy(slot.gameObject);
            }
        }
    }

    public UI_EquipmentSlot FindEquipment(EquipmentItem equipmentItem)
    {
        foreach(var slot in equipmentSlots)
        {
            if(slot.item.equipment == equipmentItem)
            {
                return slot;
            }
        }

        return null;
    }

    public virtual void SwithTo(GameObject _menu)
    {
        foreach (GameObject oldMenu in menu)
        {
            oldMenu.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);
    }
}
