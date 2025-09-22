using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Gamedata_inventory
{
    public string id;
    public bool IsCustomized;
    public SerializableDictionary<string, int> extraModife;
    public int stackSize;

    public Gamedata_inventory(string _id, SerializableDictionary<string,int> _extraModife, bool _IsCustomized,int _stack)
    {
        id = _id;
        extraModife = _extraModife;
        IsCustomized = _IsCustomized;
        stackSize = _stack;
    }
}

[System.Serializable]
public struct Gamedata_equipment
{
    public string id;
    public bool IsCustomized;
    public SerializableDictionary<string, int> extraModife;
    public int stackSize;
    public int slotIndex;

    public Gamedata_equipment(string _id, SerializableDictionary<string, int> _extraModife, bool _IsCustomized,int _stack, int _slotIndex)
    {
        id = _id;
        extraModife = _extraModife;
        IsCustomized = _IsCustomized;
        stackSize = _stack;
        slotIndex = _slotIndex;
    }
}

[System.Serializable]
public class GameData 
{
    public int currency;
    public SerializableDictionary<StatName, int> playerStat;
    public SerializableDictionary<string, int> stash;
    public List<Gamedata_inventory> inventory;
    public List<Gamedata_equipment> equipment;
    public List<Skill_info> skill;
    public SerializableDictionary<string,CheckPoint_Info> checkPoints;

    public GameData()
    {
        this.currency = 0;
        playerStat = new SerializableDictionary<StatName, int>();
        stash = new SerializableDictionary<string, int>();
        inventory = new List<Gamedata_inventory>();
        equipment = new List<Gamedata_equipment>();
        skill = new List<Skill_info>();
        checkPoints = new SerializableDictionary<string, CheckPoint_Info>();
    }
}
