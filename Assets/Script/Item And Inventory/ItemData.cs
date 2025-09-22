using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
    All
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData :ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public string itemId;
    public int price;

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}


