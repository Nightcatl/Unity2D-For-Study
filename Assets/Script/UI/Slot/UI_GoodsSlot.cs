using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GoodsSlot : MonoBehaviour, IPointerDownHandler
{
    private IItem item;
    private int stackSize;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    public void OnPointerDown(PointerEventData eventData)
    {
        /*if(item is EquipmentItem equipmentItem)
        {
            UI_Trading.instance.AddCart(equipmentItem, stackSize);
            UI_Trading.instance.RemoveGoods(equipmentItem, stackSize);
        }
        else if(item is InventoryItem inventoryItem)
        {
            UI_Trading.instance.AddCart(inventoryItem, stackSize);
            UI_Trading.instance.RemoveGoods(inventoryItem, stackSize);
        }*/

        UI_Trading.instance.Check(item, this, stackSize);
    }

    public void Initialize(IItem _item, int _stackSize)
    {
        item = _item;
        stackSize = _stackSize;
        Debug.Log(_stackSize);
        image.sprite = item.Data.itemIcon;
        text.text = Mathf.Abs(stackSize).ToString();
    }
}
