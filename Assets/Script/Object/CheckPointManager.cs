using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct CheckPoint_Info
{
    public string id;
    public string sceneName;
    public Vector2 position;
    public bool active;

    public CheckPoint_Info(string _id, string _sceneName, Vector2 _position, bool _active = true)
    {
        id = _id;
        sceneName = _sceneName;
        position = _position;
        active = _active;
    }
}


public class CheckPointManager : MonoBehaviour, ISaveManager, IManager
{
    public static CheckPointManager instance;

    public SerializableDictionary<string, CheckPoint_Info> checkPointDictionary;

    public CheckPoint currentCheckPoint;
    public List<CheckPoint> checkPointList;

    public CheckPoint_Info _lastCheckPoint;
    public CheckPoint_Info _currentCheckPoint;

    public void Initialize()
    {
        if (instance == null)
            instance = this;

        checkPointDictionary = new SerializableDictionary<string, CheckPoint_Info>();
        checkPointList = new List<CheckPoint>();
    }

    private void Start()
    {

    }

    public void FindAllCheckPoints()
    {
        GameObject[] checkpoints;

        checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");

        foreach(var item in checkpoints)
        {
            checkPointList.Add(item.GetComponent<CheckPoint>());
        }
    }

    public void AddCheckPoints(CheckPoint checkPoint)
    {
        CheckPoint_Info _checkPoint = new CheckPoint_Info(checkPoint.checkPointId, checkPoint.currentScene.SceneName, checkPoint.transform.position);

        checkPointDictionary.Add(_checkPoint.id, _checkPoint);
    }

    public void EnableCheckPoint(CheckPoint checkPoint)
    {
        if(_currentCheckPoint.id != "")
        {
            _lastCheckPoint = _currentCheckPoint;
            _lastCheckPoint.active = false;
            
            if(currentCheckPoint != null)
            {
                currentCheckPoint.active = false;
                currentCheckPoint.SetAnim();
            }

            currentCheckPoint = checkPoint;
        }

        if(checkPointDictionary.TryGetValue(checkPoint.checkPointId, out CheckPoint_Info value))
        {
            _currentCheckPoint = value;
        }
        else
        {
            AddCheckPoints(checkPoint);
            _currentCheckPoint = checkPointDictionary[checkPoint.checkPointId];
        }

        checkPoint.active = true;
        checkPoint.SetAnim();

        _currentCheckPoint.active = true;
    }

    public void LoadData(GameData _data)
    {
        checkPointDictionary = _data.checkPoints;

        foreach (var checkPoint in checkPointDictionary)
        {
            if (checkPoint.Value.active == true)
            {
                _currentCheckPoint = checkPoint.Value;
            }
        }
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        _data.checkPoints = checkPointDictionary;
    }
}
