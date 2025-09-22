using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadGame_Ui;
    [SerializeField] private GameObject NewGame_Button;
    [SerializeField] private GameObject LoadGame_Button;

    public string[] SaveFile_TimeText_Content;
    public List<TextMeshProUGUI> SaveFile_TimeText = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (Manager.instance == null)
            SceneManager.LoadScene("Presistance", LoadSceneMode.Additive);
    }

    private void Start()
    {
        for(int i = 0;i < SaveFile_TimeText.Count;i++)
        {
            if (SaveManager.instance.gameData_Preview.Count > i)
            {
                SaveFile_TimeText_Content[i] = SaveManager.instance.gameData_Preview[i].time;
            }
        }
    }

    public void NewGame()
    {
        SaveManager.instance.StartGame(false);
    }

    public void GoToLoadGame()
    {
        mainMenu.SetActive(false);
        loadGame_Ui.SetActive(true);

        for(int i = 0; i < SaveFile_TimeText.Count; i++)
        {
            SaveFile_TimeText[i].text = SaveFile_TimeText_Content[i];
        }
    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        loadGame_Ui.SetActive(false);
    }

    public void LoadGame(int index)
    {
        SaveManager.instance.StartGame(true, index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
