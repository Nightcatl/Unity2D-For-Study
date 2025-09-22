using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    protected Enemy enemy => GetComponentInParent<Enemy>();

    protected int hitDir;

    protected Entity entity;


    protected virtual void AnimationTrigger()
    {
        enemy.AnimationFinshTrigger();
    }

    private void SetTakeDamage()
    {
        enemy.takeDamage = !enemy.takeDamage;
    }
}
