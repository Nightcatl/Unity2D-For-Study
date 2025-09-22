using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Quest
{
    public int Id { get; set; }

    public bool IsAccept {  get; set; }

    public bool IsDone { get; set; }

    public string Describe { get; set; }
}
