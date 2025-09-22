using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    public void Awake()
    {
        Debug.Log(1);

        if(instance == null)
            instance = this;
    }

    public void Initialize()
    {
        if (instance == null)
            instance = this;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        _data.currency = this.currency;
    }
}
