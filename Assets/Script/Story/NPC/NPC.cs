using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private bool CanTalk;
    public TextMeshProUGUI test;

    [SerializeField] private Sprite avator;

    [SerializeField] public Goods Goods;

    [SerializeField] private string Name;
    [SerializeField] private string fileName;
    [SerializeField] private string fileType;

    private void Awake()
    {
        Goods.InitializeGoods();
    }

    public void SetCanTalk(bool _canTalk)
    {
        CanTalk = _canTalk;
        test.gameObject.SetActive(CanTalk);
    }

    public void Talk()
    {
        UI_Dialog.instance.StartDialog(Name, fileName, fileType, this, avator);
        SetCanTalk(false);
    }

    public void AddOrRemoveGoods<T>(T item, int stackSize)
        where T : IItem
    {
        Goods.AddOrRemoveGoods(item, stackSize);
    }
}
