using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GoodsCheck : MonoBehaviour
{
    private IItem item;
    private int maxStackSize;
    private int inputNum;
    private object slot;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TMP_InputField inputField;

    public void Initialize(IItem _item, object _slot, int stackSzie)
    {
        item = _item;
        slot = _slot;
        maxStackSize = stackSzie;
        image.sprite = item.Data.itemIcon;
        text.text = item.Data.name;
    }

    public void CheckInput(string _input)
    {
        if(int.TryParse(inputField.text, out int input))
        {
            Debug.Log(input);

            if ((input > 0 && maxStackSize < 0) || (input < 0 && maxStackSize > 0))
                input *= -1;

            if(input > Mathf.Abs(maxStackSize))
            {
                Debug.LogError("输入数量不应大于物品数量");
                inputField.text = maxStackSize.ToString();
            }
        }
        else
            Debug.LogError("121211212");
    }

    public void EndEdit(string _input)
    {
        if (int.TryParse(inputField.text, out int input))
        {
            if ((input > 0 && maxStackSize < 0) || (input < 0 && maxStackSize > 0))
                input *= -1;

            if (input <= Mathf.Abs(maxStackSize))
            {
                inputNum = input;
            }
            else
            {
                inputNum = maxStackSize;
                inputField.text = maxStackSize.ToString();
            }
        }
    }

    public void InputDone()
    {
        if(slot is UI_GoodsSlot)
        {
            if (item is EquipmentItem equipmentItem)
            {
                UI_Trading.instance.AddCart(equipmentItem, inputNum);
                UI_Trading.instance.RemoveGoods(equipmentItem, inputNum);
            }
            else if (item is InventoryItem inventoryItem)
            {
                UI_Trading.instance.AddCart(inventoryItem, inputNum);
                UI_Trading.instance.RemoveGoods(inventoryItem, inputNum);
            }
        }
        else if(slot is UI_CartSlot)
        {
            if (item is EquipmentItem equipmentItem)
            {
                UI_Trading.instance.AddGoods(equipmentItem, inputNum);
                UI_Trading.instance.RemoveCart(equipmentItem, inputNum);
            }
            else if (item is InventoryItem inventoryItem)
            {
                UI_Trading.instance.AddGoods(inventoryItem, inputNum);
                UI_Trading.instance.RemoveCart(inventoryItem, inputNum);
            }
        }

        inputField.text = "1";
        this.gameObject.SetActive(false);
    }
}
