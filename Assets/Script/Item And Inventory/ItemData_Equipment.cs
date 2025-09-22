using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Base")]
    [SerializeField] protected int Damage;
    [SerializeField] protected int MaxHealth;
    [SerializeField] protected int Magicpoint;
    [SerializeField] protected int Vitality;

    [Header("Damage")]
    [SerializeField] protected int Strength;
    [SerializeField] protected int Intelligence;
    [SerializeField] protected int Critical;

    [Header("Defense")]
    [SerializeField] protected int Armor;
    [SerializeField] protected int MagicResistance;
    [SerializeField] protected int[] Resistance;

    [Header("Tenacity")]
    [SerializeField] protected int Poisedamage;

    [Header("Daze")]
    [SerializeField] protected int StunDamage;

    [Header("Craft")]
    public bool CanCustomized;
    public int equipmentLevel;
    public List<InventoryItem> craftingMaterials;
    public List<InventoryItem> customizedMaterials;

    public void AddModifires()
    {
        CharacterStats playerStat = PlayerManager.instance.player.stat;

        if(Damage > 0)
            playerStat.Damage.AddModifier(Damage);
        if(MaxHealth > 0)
            playerStat.MaxHealth.AddModifier(MaxHealth);
        if(Magicpoint > 0)
            playerStat.Magicpoint.AddModifier(Magicpoint);
        if(Vitality > 0)
            playerStat.Vitality.AddModifier(Vitality);

        if(Strength > 0)
            playerStat.Strength.AddModifier(Strength);
        if(Intelligence > 0)
            playerStat.Intelligence.AddModifier(Intelligence);
        if(Critical > 0)
            playerStat.Critical.AddModifier(Critical);

        if(Armor > 0)
            playerStat.Armor.AddModifier(Armor);
        if(Magicpoint > 0)
            playerStat.MagicResistance.AddModifier(MagicResistance);
        for(int i = 0; i < Resistance.Length; i++)
        {
            if(Resistance[i] > 0)
                playerStat.Resistance[i].AddModifier(Resistance[i]);
        }

        if (Poisedamage > 0)
            playerStat.Poisedamage.AddModifier(Poisedamage);
    }

    public void RemoveModifires()
    {
        CharacterStats playerStat = PlayerManager.instance.player.stat;

        if (Damage > 0)
            playerStat.Damage.RemoveModifier(Damage);
        if (MaxHealth > 0)
            playerStat.MaxHealth.RemoveModifier(MaxHealth);
        if (Magicpoint > 0)
            playerStat.Magicpoint.RemoveModifier(Magicpoint);
        if (Vitality > 0)
            playerStat.Vitality.RemoveModifier(Vitality);

        if (Strength > 0)
            playerStat.Strength.RemoveModifier(Strength);
        if (Intelligence > 0)
            playerStat.Intelligence.RemoveModifier(Intelligence);
        if (Critical > 0)
            playerStat.Critical.RemoveModifier(Critical);

        if (Armor > 0)
            playerStat.Armor.RemoveModifier(Armor);
        if (Magicpoint > 0)
            playerStat.MagicResistance.RemoveModifier(MagicResistance);
        for (int i = 0; i < Resistance.Length; i++)
        {
            if (Resistance[i] > 0)
                playerStat.Resistance[i].RemoveModifier(Resistance[i]);
        }

        if (Poisedamage > 0)
            playerStat.Poisedamage.RemoveModifier(Poisedamage);
    }
}
