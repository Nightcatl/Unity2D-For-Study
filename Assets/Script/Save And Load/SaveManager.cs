using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SaveManager : MonoBehaviour, IManager, ISaveManager
{
    public static SaveManager instance;

    [SerializeField] private string[] fileName;
    [SerializeField] private string[] fileName_preview;

    private GameData gameData;
    public List<GameData_Preview> gameData_Preview { get; private set; }
    private List<ISaveManager> saveManagers;
    private FileDataHandle dataHandle;

    private System.DateTime startTime;
    private System.DateTime endTime;

    public void Initialize()
    {
        if (instance == null)
            instance = this;

        dataHandle = new FileDataHandle(Application.persistentDataPath, fileName, fileName_preview);
        gameData_Preview = new List<GameData_Preview>();
        saveManagers = FindAllSaveManagers();

        gameData_Preview = LoadPreviewGameData();
    }

    private void Start()
    {
    }

    #region StartGame
    public void StartGame(bool IsLoadGame,int index = 0)
    {
        if(IsLoadGame)
        {
            LoadGame(index);
        }
        else
        {
            NewGame();
        }

        /*startTime = System.DateTime.Now;

        Debug.Log(startTime.ToString());*/
    }

    private void NewGame()
    {
        gameData = new GameData();

        SceneManager.UnloadSceneAsync("MainMenu");
        SceneSwapManager.instance.StartGame(false);
    }

    private void LoadGame(int index)
    {
        SceneSwapManager.instance.StartGame(true);

        StartCoroutine(WaitForLoading(index));
    }

    private IEnumerator WaitForLoading(int index)
    {
        while(PlayerManager.instance == null)
            yield return null;

        saveManagers = FindAllSaveManagers();

        gameData = dataHandle.Load(index);

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }
    #endregion

    public void SaveGame(int index = 0)
    {
        saveManagers = FindAllSaveManagers();

        GameData_Preview _gameData_Preview = new GameData_Preview();

        if (gameData_Preview.Count > index)
            _gameData_Preview = gameData_Preview[index];

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData, ref _gameData_Preview);
        }

        dataHandle.Save(gameData, _gameData_Preview, index);

        gameData_Preview =  LoadPreviewGameData();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public List<GameData_Preview> LoadPreviewGameData()
    {
        gameData_Preview.Clear();

        List<GameData_Preview> gameData_Previews = new List<GameData_Preview>();

        for(int i = 0;i < 3; i++)
        {
            GameData_Preview loadData = dataHandle.LoadPreviewGameData(i);

            if(loadData != null)
            {
                gameData_Previews.Add(loadData);
            }
                
        }

        return gameData_Previews;
    }

    public void LoadData(GameData _data)
    {
        
    }

    public void SaveData(ref GameData _data, ref GameData_Preview _data_preview)
    {
        endTime = System.DateTime.Now;

        TimeSpan startSpan = new TimeSpan(startTime.Ticks);
        TimeSpan endSpan = new TimeSpan(endTime.Ticks);

        TimeSpan passTime = startTime.Subtract(endTime).Duration();

        Debug.Log(passTime.ToString());

        _data_preview.time = (int)passTime.Hours + " : " + (int)passTime.Minutes + " : " + (int)passTime.Seconds;
    }
}
