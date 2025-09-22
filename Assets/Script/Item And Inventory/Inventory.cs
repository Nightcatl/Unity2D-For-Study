using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;
    
    private UI_Items UI_items;

    public List<InventoryItem_Equipment> inventory;
    public SerializableDictionary<EquipmentItem, InventoryItem_Equipment> inventoryDictionary;

    public List<InventoryItem> stash;
    public SerializableDictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem_Equipment> equipment;
    public Dictionary<EquipmentItem, InventoryItem_Equipment> equipmentDictionary;

    public InventoryItem[] craftEquipment;

    public UI_MaterialSeleSlot[] selectSlot;

    [Header("Material(Example)")]
    [SerializeField] private ItemData metal;
    [SerializeField] private ItemData skin;

    [Header("Data base")]
    private List<ItemData> itemDataBase;
    public List<InventoryItem> loadedStash;
    public List<InventoryItem_Equipment> loadedInventory;
    public SerializableDictionary<InventoryItem_Equipment, int> loadedEquipment;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        UI_items = UI.instance.items;

        inventoryDictionary = new SerializableDictionary<EquipmentItem, InventoryItem_Equipment>();

        stashDictionary = new SerializableDictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem_Equipment>();
        equipmentDictionary = new Dictionary<EquipmentItem, InventoryItem_Equipment>();
    }

    public void AddStash(ItemData _item, int num = 1)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            Debug.Log(1);
            value.AddStack(num);
            UI_items.UpdateStashUI(value);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
            UI_items.AddStashSlot(newItem, num);
        }
    }

    public void PickUpEquipment(ItemData_Equipment _item)
    {
        if(_item.equipmentType == EquipmentType.Flask)
        {
            foreach(InventoryItem_Equipment _equipment in equipment)
            {
                if(_equipment.equipment.Data == _item)
                {
                    _equipment.AddStack();
                    UI_items.UpdateEquipmentUI();

                    return;
                }
            }
        }

        AddInventory(new EquipmentItem(_item));
    }

    public void AddInventory(EquipmentItem newEquipment,int stackSize = 1)
    {
        if(newEquipment._Data.equipmentType == EquipmentType.Flask)
        {
            foreach (var oldEquipment in inventory)
            {
                if(oldEquipment.equipment.Data == newEquipment.Data)
                {
                    oldEquipment.AddStack();
                    UI_items.IncreseInventory(oldEquipment.equipment);

                    return;
                }
            }
        }

        InventoryItem_Equipment newItem = new(newEquipment.Data, newEquipment);
        newItem.StackSize = stackSize;

        inventory.Add(newItem);
        inventoryDictionary.Add(newEquipment, newItem);

        UI_items.AddInventorySlot(newEquipment, newItem);
    }

    public void RemoveInventory(EquipmentItem _equipment,int StackSize = 1)
    {
        if (inventoryDictionary.TryGetValue(_equipment, out InventoryItem_Equipment inventoryValue))
        {
            if (inventoryValue.StackSize <= StackSize)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(_equipment);
            }
            else
            {
                inventoryValue.RemoveStack(StackSize);
            }

            UI_items.UpdateInventoryUI();
        }
    }

    public void RemoveStash(ItemData _item,int stackSize = 1)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem Value))
        {
            if (Value.StackSize <= 1)
            {
                stash.Remove(Value);
                stashDictionary.Remove(_item);
                UI_items.UpdateStashUI(Value);
            }
            else
            {
                Value.RemoveStack(stackSize);
                UI_items.RemoveStashSlot(Value);
            }
        }
    }

    #region Save And Load

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.stash)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.StackSize = pair.Value;

                    loadedStash.Add(itemToLoad);
                }
            }
        }

        foreach (var Loadeditem in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && Loadeditem.id == item.itemId)
                {
                    Debug.Log(1);

                    InventoryItem_Equipment itemToLoad = new InventoryItem_Equipment(item, new EquipmentItem(item as ItemData_Equipment));
                    itemToLoad.equipment.IsCustomized = Loadeditem.IsCustomized; 
                    itemToLoad.equipment.extraModifiers = Loadeditem.extraModife;
                    itemToLoad.StackSize = Loadeditem.stackSize;

                    loadedInventory.Add(itemToLoad);
                }
            }
        }

        foreach (var equipment in _data.equipment)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == equipment.id)
                {
                    InventoryItem_Equipment itemToLoad = new InventoryItem_Equipment(item, new EquipmentItem(item as ItemData_Equipment));
                    itemToLoad.equipment.IsCustomized = equipment.IsCustomized;
                    itemToLoad.equipment.extraModifiers = equipment.extraModife;
                    itemToLoad.StackSize = equipment.stackSize;

                    loadedEquipment.Add(itemToLoad, equipment.slotIndex);
                }
            }
        }

        foreach (var item in loadedInventory)
        {
            AddInventory(item.equipment, item.StackSize);
        }

        foreach (var item in loadedStash)
        {
            AddStash(item.Data, item.StackSize);
        }

        foreach (KeyValuePair<InventoryItem_Equipment, int> pair in loadedEquipment)
        {
            EquipItem(pair.Key.equipment, pair.Key.StackSize, pair.Value);
        }
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        _data.stash.Clear();
        _data.inventory.Clear();
        _data.equipment.Clear();

        Debug.Log(inventoryDictionary.Count);
        Debug.Log(stashDictionary.Count);

        foreach (KeyValuePair<EquipmentItem, InventoryItem_Equipment> pair in inventoryDictionary)
        {
            Gamedata_inventory newInventory = new Gamedata_inventory(pair.Key.Data.itemId, pair.Key.extraModifiers, pair.Key.IsCustomized, pair.Value.StackSize);

            _data.inventory.Add(newInventory);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.stash.Add(pair.Key.itemId, pair.Value.StackSize);
        }

        foreach (KeyValuePair<EquipmentItem, InventoryItem_Equipment> pair in equipmentDictionary)
        {
            int slotIndex;

            UI_EquipmentSlot slot = UI_items.FindEquipment(pair.Key);
            if (slot != null)
                slotIndex = slot.slotIndex;
            else 
                slotIndex = 0;

            Gamedata_equipment newEquipment = new Gamedata_equipment(pair.Key.Data.itemId, pair.Key.extraModifiers, pair.Key.IsCustomized, pair.Value.StackSize, slotIndex);

            _data.equipment.Add(newEquipment);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
#if UNITY_EDITOR
        itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
#endif
        return itemDataBase;
    }

    #endregion

    #region Equipment

    public void EquipItem(EquipmentItem _item, int stackSize = 1, int slotIndex = 1)
    {
        InventoryItem_Equipment oldEquipment = null;
        InventoryItem_Equipment _equipment = new InventoryItem_Equipment(_item.Data, _item);

        _equipment.StackSize = stackSize;

        foreach (KeyValuePair<EquipmentItem, InventoryItem_Equipment> item in equipmentDictionary)
        {
            if (item.Key._Data.equipmentType == _item._Data.equipmentType && item.Key.slotIndex == slotIndex)
            {
                oldEquipment = item.Value;
            }
        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment,slotIndex);
        }

        equipment.Add(_equipment);
        equipmentDictionary.Add(_item, _equipment);

        _item._Data.AddModifires();
        _item.AddExtra();

        RemoveInventory(_item, stackSize);
        UI_items.RemoveInventorySlot(_item);
        UI_items.AddEquipment(_item,_equipment,slotIndex);
    }

    public void UnEquipItem(InventoryItem_Equipment oldEquipment, int stackSize, int slotIndex = 1)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment.equipment, out InventoryItem_Equipment value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipment.equipment);

            oldEquipment.equipment._Data.RemoveModifires();

            if (oldEquipment.equipment.IsCustomized)
                oldEquipment.equipment.RemoveExtra();
        }

        AddInventory(oldEquipment.equipment, oldEquipment.StackSize);
            UI_items.RemoveEquipment(oldEquipment.equipment, slotIndex);
    }

    public bool CanCraft(ItemData_Equipment _equipment)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        EquipmentItem newEquipment = null;

        if (_equipment.equipmentType == EquipmentType.Flask)
        {
            newEquipment = CheckFlask(_equipment, newEquipment);
        }
        else
        {
            newEquipment = new EquipmentItem(_equipment);
        }

        if (_equipment.CanCustomized)
        {
            newEquipment.extraModifiers = new SerializableDictionary<string, int>();
            newEquipment.IsCustomized = true;

            for (int i = 0; i < _equipment.customizedMaterials.Count; i++)
            {
                if (UI_Craft.instance.materialSlots[i].selectItem == null)
                {
                    Debug.Log("select Item");
                    return false;
                }

                Debug.Log(UI_Craft.instance.materialSlots[i].selectItem.Data.name);

                if (stashDictionary[UI_Craft.instance.materialSlots[i].selectItem.Data].StackSize < _equipment.customizedMaterials[i].StackSize)
                {
                    Debug.Log("not enough");
                    return false;
                }
                else
                {
                    ItemData_Material material = UI_Craft.instance.materialSlots[i].selectItem.Data as ItemData_Material;
                    for (int j = 0; j < material.CharacteristicName.Length; j++)
                    {
                        newEquipment.extraModifiers.Add(material.CharacteristicName[j], material.CharacteristicValue[j]);
                    }
                    materialsToRemove.Add(UI_Craft.instance.materialSlots[i].selectItem);
                }
            }                                                                                             
        }

        for (int i = 0; i < _equipment.craftingMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_equipment.craftingMaterials[i].Data, out InventoryItem value))
            {
                if (value.StackSize < _equipment.craftingMaterials[i].StackSize)
                {
                    Debug.Log("not enough");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(_equipment.craftingMaterials[i]);
                }
            }
            else
            {
                Debug.Log("not enough");
                return false;
            }
        }

        AddInventory(newEquipment);

        for (int i = 0; i < _equipment.craftingMaterials.Count + _equipment.customizedMaterials.Count; i++)
            RemoveStash(materialsToRemove[i].Data, materialsToRemove[i].StackSize);

        UI_Craft.instance.InitializeMaterialUI();
        return true;
    }

    private EquipmentItem CheckFlask(ItemData_Equipment _equipment, EquipmentItem newEquipment)
    {
        foreach (KeyValuePair<EquipmentItem, InventoryItem_Equipment> item in inventoryDictionary)
        {
            if (item.Key._Data == _equipment)
            {
                Debug.Log("1");
                newEquipment = item.Key;
                return newEquipment;
            }
        }

        foreach (KeyValuePair<EquipmentItem, InventoryItem_Equipment> item in equipmentDictionary)
        {
            if (item.Key._Data == _equipment)
            {
                Debug.Log("1");
                newEquipment = item.Key;
                return newEquipment;
            }
        }

        return (new EquipmentItem(_equipment));
    }
    #endregion
}
