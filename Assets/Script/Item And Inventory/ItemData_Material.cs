using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    metal,
    skin,
    other
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Material")]
public class ItemData_Material : ItemData
{
    public MaterialType materialType;
    public bool IsExample;

    [Header("Characteristic")]
    public string[] CharacteristicName;
    public int[] CharacteristicValue;
}
