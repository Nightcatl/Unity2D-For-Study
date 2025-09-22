using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    [SerializeField] private GameObject Presistance;

    [SerializeField] private GameObject option_Button;
    [SerializeField] private GameObject saveGame_Button;

    public void Initialize()
    {

    }

    public void GotoSave()
    {
        option_Button.SetActive(false);
        saveGame_Button.SetActive(true);
    }

    public void ReturnToUI()
    {
        option_Button.SetActive(true);
        saveGame_Button.SetActive(false);
    }

    public void SaveGame(int index = 0)
    {
        SaveManager.instance.SaveGame(index);
    }

    public void QuitAndSave()
    {
        SaveManager.instance.SaveGame();

        UI.instance.ui.SetActive(false);
        Presistance.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.instance.player.IsBusy = false;

        SceneSwapManager.instance.QuitAndSave();
    }
}
