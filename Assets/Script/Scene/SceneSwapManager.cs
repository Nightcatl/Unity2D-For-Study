using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour, IManager
{
    [SerializeField] private GameObject Presistance;

    public static SceneSwapManager instance;

    private Player player;
    private Vector3 playerSpawnPosition;

    [SerializeField] private List<SceneField> AllScenes;

    [SerializeField] private SerializableDictionary<string, bool> door;

    [SerializeField] private List<SceneField> currentScene;
    [SerializeField] private SceneField lastScene;

    private bool CanChangePlayer;
    private bool IsSceneSwap;

    public void Initialize()
    {
        if (instance == null)
            instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region StartGame
    public void StartGame(bool IsLoadGame)
    {
        if(IsLoadGame)
        {
            LoadSceneInLoadGame();
        }
        else
        {
            LoadSceneInNewGame();
        }
    }

    private void LoadSceneInLoadGame()
    {
        SceneManager.LoadSceneAsync(AllScenes[2].SceneName, LoadSceneMode.Additive);

        StartCoroutine(WaitForPlayerManagerForLoadGame());
    }

    private IEnumerator WaitForPlayerManagerForLoadGame()
    {
        while (PlayerManager.instance == null)
        {
            yield return null;
        }

        player = PlayerManager.instance.player;

        if (CheckPointManager.instance._currentCheckPoint.sceneName != "")
        {
            CanChangePlayer = true;

            SceneManager.UnloadSceneAsync(AllScenes[0]);
            SceneManager.LoadScene(CheckPointManager.instance._currentCheckPoint.sceneName, LoadSceneMode.Additive);

            foreach (var scene in AllScenes)
            {
                if (scene.SceneName == CheckPointManager.instance._currentCheckPoint.sceneName)
                    currentScene[0] = scene;
            }
        }
        else
        {
            SceneManager.UnloadSceneAsync(AllScenes[0]);
            SceneManager.LoadScene(AllScenes[3].SceneName, LoadSceneMode.Additive);
            player.transform.position = new Vector2(-47, -8);

            currentScene[0] = AllScenes[3];
        }

        while (CameraFollow.instance == null)
        {
            yield return null;
        }
        IsSceneSwap = true;

        UI.instance.UI_InGame.SetActive(true);
    }

    private void LoadSceneInNewGame()
    {
        SceneManager.LoadScene(AllScenes[2].SceneName, LoadSceneMode.Additive);

        StartCoroutine(WaitForPlayerManagerForNewGame());
    }

    private IEnumerator WaitForPlayerManagerForNewGame()
    {
        while (PlayerManager.instance == null)
        {
            yield return null; 
        }

        player = PlayerManager.instance.player;

        SceneManager.LoadScene(AllScenes[3].SceneName, LoadSceneMode.Additive);

        while(CameraFollow.instance == null)
        {
            yield return null;
        }

        currentScene.Add(AllScenes[3]);
        IsSceneSwap = true;
        player.transform.position = new Vector2(-47, -8);
        UI.instance.UI_InGame.SetActive(true);
    }
    #endregion

    public void SceneSwap(SceneField loadScene,bool CanChangePlayer = true,bool IsSceneSwap = true)
    {
        this.CanChangePlayer = CanChangePlayer;
        this.IsSceneSwap = IsSceneSwap;

        lastScene = currentScene[0];
        currentScene.Remove(lastScene);
        currentScene.Add(loadScene);

        StartCoroutine(SceneSwapFor(loadScene));
    }

    public void SceneToLoad(SceneField loadScene)
    {
        this.CanChangePlayer = false;

        StartCoroutine(SceneSwapFor(loadScene));
    }

    private IEnumerator SceneSwapFor(SceneField loadScene)
    {
        if(CanChangePlayer)
        {
            SceneFadeManager.instance.StartFadeOut();

            while (SceneFadeManager.instance.IsFadingOut)
            {
                yield return null;
            }

            SceneManager.UnloadSceneAsync(lastScene);
        }

        SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CameraListManager.instance != null)
        {
            CameraManager.instance.FindAllVirtualCamera();
        }

        if (!IsSceneSwap)
            return;

        if (!CanChangePlayer)
            return;

        SceneFadeManager.instance.StartFadeIn();
        CheckPointManager.instance.FindAllCheckPoints();

        if (lastScene != null || lastScene.SceneName != "")
        {
            Debug.Log("Enter " + currentScene[0].SceneName);

            if (currentScene[0].SceneName == "Frost-Room" && lastScene.SceneName == "Frost-Start")
            {
                player.transform.position = new Vector2(-6, -3);
            }
            else if (currentScene[0].SceneName == "Frost-Start" && lastScene.SceneName == "Frost-Room")
            {
                player.transform.position = new Vector2(9, 9);
            }
            else if(lastScene.SceneName == "Frost-Start" && currentScene[0].SceneName == "Cave-Start")
            {
                player.transform.position = new Vector2(88, -22);
            }
        }

        if(lastScene.SceneName == "" && currentScene[0].SceneName != "")
        {
            if (CheckPointManager.instance.checkPointList.Count > 0)
                player.transform.position = CheckPointManager.instance._currentCheckPoint.position;
        }
    }

    public void QuitAndSave()
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if(scene.name != AllScenes[1].SceneName)
                SceneManager.UnloadSceneAsync(scene.name);
        }

        currentScene.Clear();
        lastScene = null;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void CurrentScene(SceneField _currentScene)
    {
        currentScene.Add(_currentScene);
    }

    public void RemoveScene(SceneField _currentScene)
    {
        currentScene.Remove(_currentScene);
    }
}
