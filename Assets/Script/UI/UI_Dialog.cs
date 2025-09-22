using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : MonoBehaviour
{
    public static UI_Dialog instance;

    private NPC npc;

    [SerializeField] private GameObject Background_content;
    [SerializeField] private TextMeshProUGUI text_content;

    [SerializeField] private GameObject Background_branch;
    [SerializeField] private GameObject ChooseDialogPrefab;

    [SerializeField] private List<GameObject> _Avators = new List<GameObject>();
    private List<Sprite> Avators = new List<Sprite>();

    private DialogDataHandle dialogDataHandle;
    private DialogData dialogData;
    private DialogData lastDialogData;
    private List<DialogData> dialogData_branch = new List<DialogData>();

    private bool EndTalk;
    private bool Choosing;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
            instance = this;
    }

    public void SetAvator(Sprite avator)
    {
        Avators.Add(avator);
    }

    public void StartDialog(string Name, string fileName, string fileType, NPC speaker, Sprite avator)
    {
        dialogDataHandle = new DialogDataHandle(fileName, Name, fileType);

        dialogDataHandle.LoadDialogFile();
        dialogData = dialogDataHandle.StartDialog(1);

        npc = speaker;

        Avators.Add(avator);

        for(int i = 0; i < _Avators.Count; i++)
        {
            _Avators[i].GetComponent<SpriteRenderer>().sprite = Avators[i];
        }

        EndTalk = false;
        ShowDialog();

        Background_content.SetActive(true);
        text_content.gameObject.SetActive(true);
        
        PlayerManager.instance.player.IsBusy = true;
        PlayerManager.instance.player.IsTalk = true;
    }

    public void ShowDialog()
    {
        Debug.Log(dialogData.Action);

        if (dialogData.Num_Branch == 1)
        {
            text_content.text = dialogData.Speaker + " : " + dialogData.Content;
            dialogDataHandle.ShowDialog(dialogData);
        }
        else if(dialogData.Num_Branch > 1)
        {
            Choosing = true;
            Background_branch.SetActive(true);

            text_content.text = dialogData.Speaker + " : " + dialogData.Content;
            dialogDataHandle.ShowDialog(dialogData);

            for (int i = 0;  i < dialogData.Num_Branch; i++)
            {
                int index = i;

                DialogData nextDialog = dialogDataHandle.ContinueDialog(dialogData.NextId[0] + i);

                dialogData_branch.Add(nextDialog);
                GameObject chooseDialog = Instantiate(ChooseDialogPrefab, Background_branch.transform);

                if(nextDialog.Action == 1)
                {
                    chooseDialog.GetComponent<Button>().onClick.AddListener(() => Trading(npc));
                    Debug.Log(1);
                }

                chooseDialog.GetComponent<Button>().onClick.AddListener(() => ChooseDialog(index));
                
                chooseDialog.GetComponentInChildren<TextMeshProUGUI>().text = i + "." + dialogData_branch[i].Content;
            }
        }
    }

    public void ChooseDialog(int i)
    {
        dialogData = dialogData_branch[i];

        foreach(Transform child in Background_branch.transform)
            Destroy(child.gameObject);
        Background_branch.SetActive(false);
        Choosing = false;

        if (dialogData.NextId[0] < 0)
        {
            EndTalk = true;
        }

        ShowDialog();
    }

    public void Trading(NPC npc)
    {
        UI_Trading.instance.gameObject.SetActive(true);
        UI_Trading.instance.StartTrading(npc);
    }

    public bool ContinueDialog()
    {
        if (Choosing)
        {
            ChooseDialog(0);
            return false;
        }

        if (EndTalk)
        {
            EndDialog();
            return false;
        }

        lastDialogData = dialogData;
        dialogData = dialogDataHandle.ContinueDialog(dialogData.NextId[0]);
        ShowDialog();

        if(dialogData.NextId[0] < 0)
        {
            Debug.Log("1");
            EndTalk = true;
        }
            

        return true;
    }

    public void EndDialog()
    {
        Debug.Log("½áÊø¶Ô»°");

        dialogDataHandle.EndDialog();
        dialogDataHandle = null;

        text_content.gameObject.SetActive(false);
        Background_content.SetActive(false);

        PlayerManager.instance.player.IsBusy = false;
        PlayerManager.instance.player.IsTalk = false;

        npc.SetCanTalk(true);
    }
}
