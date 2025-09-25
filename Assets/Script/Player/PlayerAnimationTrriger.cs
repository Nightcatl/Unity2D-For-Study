using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrriger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private GameObject[] particles;

    private void AnimationTrriger()
    {
        player.AnimationTrriger();
    }

    private void SetTakeDamage()
    {
        if(!player.takeDamage)
        {
            player.StartAttack();
        }
        else
        {
            player.EndAttack();
        }

        player.takeDamage = !player.takeDamage;
    }

    private void SetInvincibility()
    {
        player.SetInvincibility();
    }

    private void RemoveInvincibility()
    {
        player.RemoveInvincibility();
    }

    private void ChangeColor()
    {
        player.sr.material.SetColor("_SwordColor", player.changeColor[player.ColorIndex]);
    }

    private void ReturnColor()
    {
        player.sr.material.SetColor("_SwordColor", player.defaultColor);
    }

    private void ScreenShake()
    {
        player.fx.ScreenShake();
    }
}
