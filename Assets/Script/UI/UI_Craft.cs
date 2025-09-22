using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class UI_Craft : MonoBehaviour
{
    public static UI_Craft instance;

    private List<UI_CraftSlot> craftSlots;
    private List<UI_MaterialSeleSlot> materialSelectSlots;
    public UI_MaterialSlot[] materialSlots;
    public UI_ItemSlot itemslot;

    [SerializeField] private GameObject craftSlotsPrefab;
    [SerializeField] private GameObject materialSelectSlotPrefab;

    [SerializeField] private GameObject craftSlotParent;
    [SerializeField] private GameObject materialSlotParent;
    [SerializeField] private GameObject materialSelectParent;

    public void Initialize()
    {
        if (instance == null)
            instance = this;

        craftSlots = new List<UI_CraftSlot>();
        materialSelectSlots = new List<UI_MaterialSeleSlot>();

        materialSlots = materialSlotParent.GetComponentsInChildren<UI_MaterialSlot>();
    }

    protected void Start()
    {
        for(int i = 0; i <  Inventory.instance.craftEquipment.Length; i++)
        {
            AddCraftSlot(Inventory.instance.craftEquipment[i]);
        }

        UpdateCraftMaterialUI(craftSlots[0].item);
    }

    public void InitializeMaterialUI()
    {
        foreach(UI_MaterialSlot slot in materialSlots)
        {
            slot.item = null;
            slot.selectItem = null;

            slot.UpdataSlotUI();
        }
    }

    public void UpdateCraftEquipmentUI()
    {
        foreach(UI_CraftSlot slot in craftSlots)
        {
            slot.UpdateCraftEquipmentUI();
        }
    }

    public void UpdateCraftMaterialUI(InventoryItem _equipment)
    {
        int i = 0;
        ItemData_Equipment equipment = _equipment.Data as ItemData_Equipment;

        itemslot.item = _equipment;
        itemslot.UpdataSlotUI();

        for (int j = 0; j < 3; j++)
        {
            materialSlots[j].item = null;
        }

        foreach (InventoryItem _item in equipment.customizedMaterials)
        {
            materialSlots[i].item = _item;
            i++;
        }

        foreach(InventoryItem _item in equipment.craftingMaterials)
        {
            materialSlots[i].item = _item;
            i++;
        }

        for(int j = 0;j < 3; j++)
        {
            materialSlots[j].UpdataSlotUI();
        }
    }

    public void AddCraftSlot(InventoryItem craftEquipment)
    {
        if (craftEquipment == null)
            return;

        GameObject craftSlot = Instantiate(craftSlotsPrefab, craftSlotParent.transform);

        UI_CraftSlot ui_CraftSlot = craftSlot.GetComponent<UI_CraftSlot>();

        craftSlots.Add(ui_CraftSlot);

        craftSlots[craftSlots.Count - 1].item = craftEquipment;

        craftSlots[craftSlots.Count - 1].UpdateCraftEquipmentUI();
    }

    public void AddMaterialSelectSlot(ItemData_Material material, UI_MaterialSlot materialSlot)
    {
        MaterialType materialType = material.materialType;

        foreach(KeyValuePair<ItemData, InventoryItem> item in Inventory.instance.stashDictionary)
        {
            ItemData_Material stash = item.Key as ItemData_Material;
            if(materialType == stash.materialType)
            {
                GameObject materialSelectSlot = Instantiate(materialSelectSlotPrefab, materialSelectParent.transform);
                UI_MaterialSeleSlot ui_MaterialSeleSlot = materialSelectSlot.GetComponent<UI_MaterialSeleSlot>();

                ui_MaterialSeleSlot.item = item.Value;
                ui_MaterialSeleSlot.slot = materialSlot;
                ui_MaterialSeleSlot.UpdataSlotUI();

                materialSelectSlots.Add(ui_MaterialSeleSlot);
            }
        }

        if (materialSelectSlots.Count == 0)
            Debug.Log("not enough");
    }

    public void RemoveMaterialSelectSlot()
    {
        for(int i = 0; i < materialSelectSlots.Count; i++)
        {
            UI_MaterialSeleSlot ui_MaterialSeleSlot = materialSelectSlots[i];
            materialSelectSlots.Remove(materialSelectSlots[i]);
            Destroy(ui_MaterialSeleSlot.gameObject);
        }
    }

    public void Craft()
    {
        Inventory.instance.CanCraft(itemslot.item.Data as ItemData_Equipment);
    }
}
