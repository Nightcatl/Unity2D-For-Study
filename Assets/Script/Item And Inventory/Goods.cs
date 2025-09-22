using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Goods
{
    [SerializeField] private List<EquipmentItem> equipmentItems;
    [SerializeField] private List<InventoryItem> inventoryItems;

     private List<IItem> items;

    public void InitializeGoods()
    {
        items = new List<IItem>();

        if (equipmentItems != null)
        {
            foreach (var item in equipmentItems)
            {
                items.Add(item);
            }
        }

        if (inventoryItems != null)
        {
            foreach (var item in inventoryItems)
            {
                items.Add(item);
            }
        }
    }

    public object GetGoods(ItemType type)
    {
        switch (type)
        {
            case ItemType.Equipment:
                return equipmentItems;
            case ItemType.Material:
                return inventoryItems;
            case ItemType.All:
                return items;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), "非法类型");
        }
    }

    public void AddOrRemoveGoods<T>(T item, int stackSize)
        where T : IItem
    {
        if (item == null)
            Debug.LogError("物品不能为空");

        if (stackSize == 0)
            Debug.LogError("数值不能为0");

        if (stackSize > 0)
        {
            if (item is EquipmentItem equipmentItem)
                HandleItemAddal(equipmentItems, equipmentItem, stackSize, ItemType.Equipment);
            else if (item is InventoryItem inventoryItem)
                HandleItemAddal(inventoryItems, inventoryItem, stackSize, ItemType.Material);
            else
                Debug.LogError("未知类型");
        }
        else
        {
            if (item is EquipmentItem equipmentItem)
                HandleItemRemoveal(equipmentItems, equipmentItem, stackSize, ItemType.Equipment);
            else if (item is InventoryItem inventoryItem)
                HandleItemRemoveal(inventoryItems, inventoryItem, stackSize, ItemType.Material);
            else
                Debug.LogError("未知类型");
        }
    }

    private void HandleItemRemoveal<T>(List<T> list, T itemToRemove, int RemoveCount, ItemType type)
        where T : IItem
    {
        if (type == ItemType.Equipment)
        {
            foreach (var item in list)
            {
                if (item.Equals(itemToRemove))
                {
                    list.Remove(itemToRemove);
                    return;
                }
            }

            Debug.LogError("未找到符合的装备");
        }
        else if(type == ItemType.Material)
        {
            foreach(var item in list)
            {
                if (item.Equals(itemToRemove))
                {
                    item.StackSize += RemoveCount;
                    if(item.StackSize <= 0)
                        list.Remove(itemToRemove);
                }
            }
        }
    }

    private void HandleItemAddal<T>(List<T> list, T itemToAdd, int AddCount, ItemType type)
        where T : IItem
    {
        if (type == ItemType.Equipment)
        {
            list.Add(itemToAdd);

            if (AddCount > 1)
                Debug.LogError("装备不允许堆叠，数量应为1");
        }
        else if (type == ItemType.Material)
        {
            foreach (var _item in list)
            {
                if (_item.Equals(itemToAdd))
                {
                    _item.StackSize += AddCount;
                    return;
                }
            }

            list.Add(itemToAdd);
        }
    }
}
