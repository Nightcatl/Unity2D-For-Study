using System;
using UnityEngine;

[Serializable]
public class EquipmentItem : IItem
{
    [SerializeField] public ItemData_Equipment _Data;

    [SerializeField] private int _StackSize;

    public ItemData Data => _Data;

    public int StackSize
    {
        get => _StackSize;
        set => _StackSize = value;
    }

    public int slotIndex = 0;

    [Header("Customized")]
    public SerializableDictionary<string, int> extraModifiers;
    public bool IsCustomized;

    public EquipmentItem(ItemData_Equipment _item)
    {
        _Data = _item;
    }

    public void AddStack(int num)
    {
        throw new ArgumentOutOfRangeException("װ�����ܱ��ѵ�,��Ϊ�Ƿ�����");
    }

    public void RemoveStack(int num)
    {
        throw new ArgumentOutOfRangeException("װ�����ܱ��ѵ�����Ϊ�Ƿ�����");
    }

    public void AddExtra()
    {

    }

    public void RemoveExtra()
    {

    }
}