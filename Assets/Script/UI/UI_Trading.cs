using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UI_Trading : MonoBehaviour
{
    public static UI_Trading instance;

    private bool isNPCShop;
    private bool isStartShoping;

    [SerializeField] private GameObject CheckInput;

    [SerializeField] private TextMeshProUGUI Text_Price;
    private int currentPrice;

    [SerializeField] private GameObject GoodsSlot;
    [SerializeField] private GameObject GoodsSlotParent_Player;
    [SerializeField] private GameObject GoodsSlotParent_NPC;

    [SerializeField] private GameObject CartSlot;
    [SerializeField] private GameObject CartSlotParent;

    [SerializeField] private GameObject NPC;

    public NPC npc;

    public Dictionary<IItem, GameObject> goodsSlots_NPC;
    public SerializableDictionary<EquipmentItem, int> equipmentItems_NPC;
    public SerializableDictionary<InventoryItem, int> inventoryItems_NPC;
    public SerializableDictionary<IItem, int> items_NPC;

    public Dictionary<IItem, GameObject> goodsSlots_Player;
    public SerializableDictionary<EquipmentItem, int> equipmentItems_Player;
    public SerializableDictionary<InventoryItem, int> inventoryItems_Player;
    public SerializableDictionary<IItem, int> items_Player;

    public Dictionary<IItem, GameObject> cartSlots;
    public SerializableDictionary<EquipmentItem, int> cart_equipmentItem;
    public SerializableDictionary<InventoryItem, int> cart_inventoryItem;
    public SerializableDictionary<IItem, int> cart;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        isNPCShop = true;

        goodsSlots_Player = new Dictionary<IItem, GameObject>();
        equipmentItems_Player = new SerializableDictionary<EquipmentItem, int>();
        inventoryItems_Player = new SerializableDictionary<InventoryItem, int>();
        items_Player = new SerializableDictionary<IItem, int>();

        goodsSlots_NPC = new Dictionary<IItem, GameObject>();
        equipmentItems_NPC = new SerializableDictionary<EquipmentItem, int>();
        inventoryItems_NPC = new SerializableDictionary<InventoryItem, int>();
        items_NPC = new SerializableDictionary<IItem, int>();

        cartSlots = new Dictionary<IItem, GameObject>();
        cart_equipmentItem = new SerializableDictionary<EquipmentItem, int>();
        cart_inventoryItem = new SerializableDictionary<InventoryItem, int>();
        cart = new SerializableDictionary<IItem, int>();
    }

    public void StartTrading(NPC _npc)
    {
        Clear();
        npc = _npc;

        currentPrice = 0;
        Text_Price.text = Regex.Replace(Text_Price.text, @"[+-]?\d+(\.\d+)?", "0");

        GetPlayerGoods();
        GetNPCGoods();

        isStartShoping = true;
    }

    private void Clear()
    {
        goodsSlots_Player.Clear();
        equipmentItems_Player.Clear();
        inventoryItems_Player.Clear();
        items_Player.Clear();

        goodsSlots_NPC.Clear();
        equipmentItems_NPC.Clear();
        inventoryItems_NPC.Clear();
        items_NPC.Clear();

        cartSlots.Clear();
        cart_equipmentItem.Clear();
        cart_inventoryItem.Clear();
        cart.Clear();
    }

    #region goods
    public void AddGoods(EquipmentItem goods, int stackSize)
    {
        if (stackSize > 0)
        {
            equipmentItems_NPC.Add(goods, goods.StackSize * stackSize);
            items_NPC.Add(goods, goods.StackSize * stackSize);

            GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_NPC.transform);
            UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();
            _slot.Initialize(goods, stackSize);

            goodsSlots_NPC.Add(goods, slot);
        }
        else if (stackSize < 0)
        {
            equipmentItems_Player.Add(goods, goods.StackSize * stackSize);
            items_Player.Add(goods, goods.StackSize * stackSize);

            GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_Player.transform);
            UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();
            _slot.Initialize(goods, stackSize);

            goodsSlots_Player.Add(goods, slot);
        }
        else if (stackSize == 0)
            Debug.LogError("商品数量不能为0，数据异常");
        
    }

    public void AddGoods(InventoryItem goods, int stackSize)
    {
        if(stackSize > 0)
        {
            if (inventoryItems_NPC.ContainsKey(goods))
            {
                inventoryItems_NPC[goods] += stackSize;
                items_NPC[goods] += stackSize;
                goodsSlots_NPC[goods].GetComponent<UI_GoodsSlot>().Initialize(goods, inventoryItems_NPC[goods]);
            }
            else
            {
                inventoryItems_NPC.Add(goods, stackSize);
                items_NPC.Add(goods, stackSize);

                GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_NPC.transform);
                UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();
                _slot.Initialize(goods, stackSize);

                goodsSlots_NPC.Add(goods, slot);
            }
        }
        else if(stackSize < 0)
        {
            if (inventoryItems_NPC.ContainsKey(goods))
            {
                inventoryItems_NPC[goods] -= stackSize;
                items_NPC[(goods)] -= stackSize;
                goodsSlots_Player[goods].GetComponent<UI_GoodsSlot>().Initialize(goods, inventoryItems_Player[goods]);
            }
            else
            {
                inventoryItems_Player.Add(goods, stackSize);
                items_Player.Add(goods, stackSize);

                GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_Player.transform);
                UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();
                _slot.Initialize(goods, stackSize);

                goodsSlots_Player.Add(goods, slot);
            }
        }
    }

    public void GetNPCGoods()
    {
        isNPCShop = true;

        GoodsSlotParent_Player.SetActive(false);
        GoodsSlotParent_NPC.SetActive(true);

        if(!isStartShoping)
        {
            while (GoodsSlotParent_NPC.transform.childCount > 0)
            {
                Transform child = GoodsSlotParent_NPC.transform.GetChild(0);

                child.SetParent(null);

                Destroy(child.gameObject);
            }

            foreach (var item in (List<EquipmentItem>)npc.Goods.GetGoods(ItemType.Equipment))
            {
                equipmentItems_NPC.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
                items_NPC.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
            }

            foreach (var item in (List<InventoryItem>)npc.Goods.GetGoods(ItemType.Material))
            {
                inventoryItems_NPC.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
                items_NPC.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
            }

            foreach (var item in items_NPC)
            {
                GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_NPC.transform);
                UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();

                _slot.Initialize(item.Key, item.Value);
                goodsSlots_NPC.Add(item.Key, slot);
            }
        }
    }

    public void GetPlayerGoods()
    {
        isNPCShop = false;

        GoodsSlotParent_NPC.SetActive(false);
        GoodsSlotParent_Player.SetActive(true);

        if(!isStartShoping)
        {
            while (GoodsSlotParent_Player.transform.childCount > 0)
            {
                Transform child = GoodsSlotParent_Player.transform.GetChild(0);

                child.SetParent(null);

                Destroy(child.gameObject);
            }

            foreach (var item in Inventory.instance.inventory)
            {
                equipmentItems_Player.Add(item.equipment, item.StackSize * (isNPCShop ? 1 : -1));
                items_Player.Add(item.equipment, item.StackSize * (isNPCShop ? 1 : -1));
            }

            foreach (var item in Inventory.instance.stash)
            {
                inventoryItems_Player.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
                items_Player.Add(item, item.StackSize * (isNPCShop ? 1 : -1));
            }

            foreach (var item in items_Player)
            {
                GameObject slot = Instantiate(GoodsSlot, GoodsSlotParent_Player.transform);
                UI_GoodsSlot _slot = slot.GetComponent<UI_GoodsSlot>();

                _slot.Initialize(item.Key, item.Value);
                goodsSlots_Player.Add(item.Key, slot);
            }
        } 
    }

    public void RemoveGoods(EquipmentItem goods, int stackSize)
    {
        if(stackSize > 0)
        {
            equipmentItems_NPC.Remove(goods);
            items_NPC.Remove(goods);
            GameObject itemToRemove = goodsSlots_NPC[goods];
            goodsSlots_NPC.Remove(goods);
            Destroy(itemToRemove.gameObject);
        }
        else if(stackSize < 0)
        {
            equipmentItems_Player.Remove(goods);
            items_Player.Remove(goods);
            GameObject itemToRemove = goodsSlots_Player[goods];
            goodsSlots_Player.Remove(goods);
            Destroy(itemToRemove.gameObject);
        }
    }

    public void RemoveGoods(InventoryItem goods, int stackSize)
    {
        if (stackSize > 0)
        {
            if(inventoryItems_NPC.ContainsKey(goods))
            {
                if(stackSize < inventoryItems_NPC[goods])
                {
                    inventoryItems_NPC[goods] -= stackSize;
                    items_NPC[goods] -= stackSize;
                    //Debug.Log(inventoryItems_NPC[goods]);
                    goodsSlots_NPC[goods].GetComponent<UI_GoodsSlot>().Initialize(goods, inventoryItems_NPC[goods]);
                }
                else
                {
                    inventoryItems_NPC.Remove(goods);
                    items_NPC.Remove(goods);
                    GameObject itemToRemove = goodsSlots_NPC[goods];
                    goodsSlots_NPC.Remove(goods);
                    Destroy(itemToRemove.gameObject);
                }
            }
        }
        else if(stackSize < 0)
        {
            if (inventoryItems_Player.ContainsKey(goods))
            {
                if (stackSize < inventoryItems_Player[goods])
                {
                    inventoryItems_Player[goods] += stackSize;
                    items_Player[goods] += stackSize;
                    goodsSlots_Player[goods].GetComponent<UI_GoodsSlot>().Initialize(goods,inventoryItems_Player[goods]);
                } 
                else
                {
                    inventoryItems_Player.Remove(goods);
                    items_Player.Remove(goods);
                    GameObject itemToRemove = goodsSlots_Player[goods];
                    goodsSlots_Player.Remove(goods);
                    Destroy(itemToRemove.gameObject);
                }
            }
        }
    }
    #endregion

    #region Cart
    public void CalculatePrice(IItem goods, int stackSize)
    {
        int _price = goods.Data.price * stackSize;

        string text = Text_Price.text;

        currentPrice += _price;

        string updatedText = Regex.Replace(text, @"[+-]?\d+(\.\d+)?", currentPrice.ToString());
        Text_Price.text = updatedText;
    }

    public void AddCart(EquipmentItem goods, int stackSize)
    {
        cart_equipmentItem.Add(goods, goods.StackSize * (isNPCShop ? 1 : -1));
        cart.Add(goods, goods.StackSize * (isNPCShop ? 1 : -1));

        GameObject slot = Instantiate(CartSlot, CartSlotParent.transform);
        UI_CartSlot _slot = slot.GetComponent <UI_CartSlot>();
        _slot.Initialize(goods, stackSize);

        cartSlots.Add(goods, slot);

        CalculatePrice(goods, stackSize);
    }

    public void AddCart(InventoryItem goods, int stackSize)
    {
        if(cart_inventoryItem.ContainsKey(goods))
        {
            cart_inventoryItem[goods] += stackSize;
            cartSlots[goods].GetComponent<UI_CartSlot>().Initialize(goods, cart_inventoryItem[goods]);

            CalculatePrice(goods, stackSize);
        }
        else
        {
            cart_inventoryItem.Add(goods, stackSize * (isNPCShop ? 1 : -1));
            cart.Add(goods, stackSize * (isNPCShop ? 1 : -1));

            GameObject slot = Instantiate(CartSlot, CartSlotParent.transform);
            UI_CartSlot _slot = slot.GetComponent<UI_CartSlot>();
            _slot.Initialize(goods, stackSize);

            cartSlots.Add(goods, slot);

            CalculatePrice(goods, stackSize);
        }  
    }

    public void RemoveCart(EquipmentItem goods, int stackSize)
    {
        cart_equipmentItem.Remove(goods);
        cart.Remove(goods);
        GameObject itemToRemove = cartSlots[goods];
        cartSlots.Remove(goods);
        Destroy(itemToRemove.gameObject);

        CalculatePrice(goods, -stackSize);
    }

    public void RemoveCart(InventoryItem goods, int stackSize)
    {

        if (cartSlots.ContainsKey(goods))
        {
            if (Mathf.Abs(stackSize) < cart_inventoryItem[goods])
            {
                Debug.Log("cart_inventoryItem[goods]: " + cart_inventoryItem[goods] + "stackSize: " + stackSize);
                cart_inventoryItem[goods] -= Mathf.Abs(stackSize);
                Debug.Log(cart_inventoryItem[goods]);
                cartSlots[goods].GetComponent<UI_CartSlot>().Initialize(goods, cart_inventoryItem[goods]);

                CalculatePrice(goods, -stackSize);
            }
            else
            {
                cart_inventoryItem.Remove(goods);
                cart.Remove(goods);
                GameObject itemToRemove = cartSlots[goods];
                cartSlots.Remove(goods);
                Destroy(itemToRemove.gameObject);

                CalculatePrice(goods, -stackSize);
            }
        }
    }
    #endregion

    public void Check(IItem item, object slot, int stackSize)
    {
        CheckInput.SetActive(true);
        CheckInput.GetComponent<UI_GoodsCheck>().Initialize(item, slot, stackSize);
    }

    public void BuyGoods()
    {
        foreach(var item in cart_equipmentItem.Keys)
        {
            if (cart_equipmentItem[item] > 0)
            {
                Inventory.instance.AddInventory(item, cart_equipmentItem[item]);
                npc.AddOrRemoveGoods(item, cart_equipmentItem[item] * -1);
            }
            else if (cart_equipmentItem[item] < 0)
            {
                Inventory.instance.RemoveInventory(item, item.StackSize);
                npc.AddOrRemoveGoods(item, cart_equipmentItem[item] * -1);
            }
        }

        foreach(var item in cart_inventoryItem.Keys)
        {
            if (cart_inventoryItem[item] > 0)
            {
                Inventory.instance.AddStash(item.Data, cart_inventoryItem[item]);
                npc.AddOrRemoveGoods(item, cart_inventoryItem[item] * -1);
            }
            else if (cart_inventoryItem[item] < 0)
            {
                Inventory.instance.RemoveStash(item.Data, item.StackSize);
                npc.AddOrRemoveGoods(item, cart_inventoryItem[item] * -1);
            }
        }

        isStartShoping = false;
        Clear();
        ClearAllCart();
        this.gameObject.SetActive(false);
    }

    public void ClearAllCart()
    {
        foreach(var item in cart_equipmentItem.Keys)
        { 
            AddGoods(item, cart_equipmentItem[item]);
        }

        foreach(var item in cart_inventoryItem.Keys)
        {
            AddGoods(item, cart_inventoryItem[item]);
        }

        while(CartSlotParent.transform.childCount > 0)
        {
            Transform child = CartSlotParent.transform.GetChild(0);

            child.SetParent(null);

            Destroy(child.gameObject);
        }

        currentPrice = 0;

        string updatedText = Regex.Replace(Text_Price.text, @"[+-]?\d+(\.\d+)?", currentPrice.ToString());
        Text_Price.text = updatedText;

        cartSlots.Clear();
        cart_equipmentItem.Clear();
        cart_inventoryItem.Clear();
        cart.Clear();
    }
}
