using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Request
{
    public int id;
    public string type;
    public string name;
    public int[] plotnode;
}

[System.Serializable]
public struct PlotNode
{
    public int id;
    public bool complete;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UI_InGame ui;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textParent;
    public static LevelManager instance;

    [SerializeField] private List<Request> AllRequstName;
    [SerializeField] private List<PlotNode> AllPlotNodes;

    public bool busy;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void StartBossFight(string levelName, Enemy boss, string[] context)
    {
        
        PlayerManager.instance.player.ReturnToIdle();

        ui.ShowBossHealth(boss.GetComponent<EnemyStat>());
        StartCoroutine(ShowContext(levelName, context, boss, true));
    }

    public void EndBossFight(string levelName, Enemy boss, string[] context)
    {
        PlayerManager.instance.player.ReturnToIdle();

        ui.HideBossHealth();
        StartCoroutine(ShowContext(levelName, context, boss, false));
    }

    private IEnumerator ShowContext(string levelName, string[] context, Enemy boss, bool isStart)
    {
        busy = true;
        textParent.SetActive(true);

        foreach(var line in context)
        {
            text.text = line;

            yield return new WaitForSeconds(1f);
        }

        if (isStart)
        {
            switch (levelName)
            {
                case "RedHood":
                    boss.StartToAttack();
                    break;
            }
        }

        textParent.SetActive(false);
        busy = false;

        yield return null;
    }
}
