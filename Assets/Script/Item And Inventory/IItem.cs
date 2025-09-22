using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem 
{
    public ItemData Data { get; }

    public int StackSize { get; set; }

    public void AddStack(int num);

    public void RemoveStack(int num);
}
