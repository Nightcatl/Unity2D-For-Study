using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogData
{
    public int Id;
    public List<int> NextId = new List<int>();
    public string Content;
    public string Speaker;
    public int QuestId;
    public int Num_Branch;
    public int Action;
}
