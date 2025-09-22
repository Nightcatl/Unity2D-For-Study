using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class DialogDataHandle
{
    private Dictionary<int, DialogData> dialogs = new Dictionary<int, DialogData>();

    private string[] Tags = {"<ID>","<Speaker>","<Content>","<Action>","<QuestID>","<NextID>" };
    private Dictionary<int, Action<DialogData, string>> _tagHandlers;

    private string fileName;
    private string speaker;
    private string fileType;
    private string filePath = Application.dataPath + "/Dialog";

    public DialogDataHandle(string _fileName, string _speaker, string _fileType)
    {
        fileName = _fileName;
        speaker = _speaker;
        fileType = _fileType;
        
        _tagHandlers = new Dictionary<int, Action<DialogData, string>>
        {
            {0, (dialog, value) =>
            {
                if(int.TryParse(value, out int id))
                    dialog.Id = id;
                else
                    Debug.LogError("无效的ID");
            }
            },
            {1, (dialog, value) => dialog.Speaker = value },
            {2, (dialog, value) => dialog.Content = value },
            {3, (dialogs,value) =>
            {
                if(int.TryParse(value,out int id))
                    dialogs.Action = id;
                else
                    Debug.LogError("无效行为ID");
            }
            },
            {4, (dialog, value) =>
            {
                if(int.TryParse(value,out int id))
                    dialog.QuestId = id;
                else
                    Debug.LogError("无效的任务ID");
            }
            },
            {5, HandleNextIdTag }
        };
    }

    public void LoadDialogFile()
    {

        string fullPath = Path.Combine(filePath, fileType, speaker, fileName);

        try
        {
            string[] lines = File.ReadAllLines(fullPath);

            DialogData currentDialog = new DialogData();

            foreach(string line in lines)
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("//"))
                    continue;
            
                string trimmedLine = line.Trim();

                if(trimmedLine.StartsWith("<Start>"))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("<End>"))
                {
                    if (!dialogs.ContainsKey(currentDialog.Id))
                    {
                        dialogs.Add(currentDialog.Id, currentDialog);
                        currentDialog = new DialogData();
                    }
                    else
                    {
                        Debug.LogWarning("重复对话ID");
                    }
                    continue;
                }

                ProcessTags(trimmedLine, currentDialog);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void ProcessTags(string line, DialogData dialog)
    {
        for (int i = 0; i < Tags.Length; i++)
        {
            string Tag = Tags[i];
            if (line.StartsWith(Tag))
            {
                string pattern = $@"{Regex.Escape(Tag)}(.*?){Regex.Escape(Tag)}";
                Match match = Regex.Match(line, pattern, RegexOptions.Singleline);

                if (match.Success && match.Groups.Count > 1)
                {
                    string value = match.Groups[1].Value.Trim();

                    if (_tagHandlers.TryGetValue(i, out var handler))
                    {
                        handler.Invoke(dialog, value);
                    }
                    else
                    {
                        Debug.LogWarning($"未找到标签 {Tag} 的处理方法");
                    }
                }
                else
                {
                    Debug.LogWarning($"无法解析标签内容: {line}");
                }
                break;
            }
        }
    }

    private void HandleNextIdTag(DialogData dialog, string value)
    {
        dialog.NextId.Clear();

        string[] separators = { ",", "，", "|", " " };
        string[] nextIdParts = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in nextIdParts)
        {
            string trimmedPart = part.Trim();
            if (int.TryParse(trimmedPart, out int nextId))
            {
                dialog.NextId.Add(nextId);
            }
            else
            {
                Debug.LogWarning("出现无效ID");
            }
        }

        dialog.Num_Branch = dialog.NextId.Count;

        if (dialog.Num_Branch == 0)
        {
            Debug.LogWarning("缺少后续ID");
        }
    }

    public DialogData GetDialog(int id)
    {
        if (dialogs.TryGetValue(id, out DialogData dialog))
        {
            return dialog;
        }
        return null;
    }

    public DialogData StartDialog(int startId)
    {
        Debug.Log("Start Dialog");

        DialogData currentDialog = GetDialog(startId);

        if (currentDialog != null)
        {
            return currentDialog;
        }
        else
        {
            return null;
        }
    }

    public void ShowDialog(DialogData dialog)
    {
        Debug.Log($"{dialog.Speaker}: {dialog.Content}");
    }

    public DialogData ContinueDialog(int id)
    {
        return GetDialog(id);
    }

    public void EndDialog()
    {
        Debug.Log("Dialog ended");
    }
}
