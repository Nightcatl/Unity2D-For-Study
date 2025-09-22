using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    [SerializeField] private UI_Characte ui_Characte;

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();

        if(currentMagicpoint < Magicpoint.GetValue())
        {
            IncreseMagicPoint(0.1f * Time.deltaTime);          
        }
    }

    public override void SetStatVaule(StatName type, int Num)
    {
        base.SetStatVaule(type, Num);

        UI.instance.menu[0].GetComponent<UI_Characte>().UpdateStatSlot();
    }

    public override void Slowdown(float _num)
    {
        base.Slowdown(_num);

        player.jumpForce *= _num;
    }

    public override void ReturnDefaultMoveSpeed()
    {
        base.ReturnDefaultMoveSpeed();

        player.jumpForce = player.defaultJumpForce;
    }
}
