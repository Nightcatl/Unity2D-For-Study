using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour ,IPointerDownHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText_Amount;
    
    public InventoryItem item;

    private void Awake()
    {
        
    }

    protected virtual void Start()
    {

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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }

    public void SetImage(Image _newImage)
    {
        itemImage.sprite = _newImage.sprite;
    }
}
