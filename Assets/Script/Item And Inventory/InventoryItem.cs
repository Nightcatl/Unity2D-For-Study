using System;
using UnityEngine;

[Serializable]
public class InventoryItem : IItem
{
    [SerializeField] private ItemData _Data;

    [SerializeField] private int _StackSize;

    public ItemData Data => _Data;

    public int StackSize
    {
        get => _StackSize;
        set => _StackSize = value;
    }

    //public ItemData Data { get; }

    //public int StackSize { get; set; }

    public InventoryItem(ItemData _newItemData)
    {
        _Data = _newItemData;
    }

    public void AddStack(int num = 1)
    {
        StackSize += num;
    }

    public void RemoveStack(int num = 1)
    {
        StackSize -= num;
    }
}
