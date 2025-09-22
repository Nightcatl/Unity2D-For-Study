using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUsingMagicState : PlayerState
{
    

    public PlayerUsingMagicState(Player _player, PlayerStateMachine _stateMachine, string _animBollName) : base(_player, _stateMachine, _animBollName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        player.IsOver = false;
        UseSkill();
    }
    
    private void UseSkill()
    {
        switch (player.magicType.attributeType)
        {
            case AttributeType.Fire:
                SkillManager.instance.fire.UseSkill();
                break;
            case AttributeType.Water:
                SkillManager.instance.water.UseSkill();
                break;
            case AttributeType.Thunder:
                SkillManager.instance.thunder.UseSkill();
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsAim)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                UseSkill();
            }
        }

        if (player.IsOver  && !player.IsAim)
        {
            stateMachine.ChangeState(player.idleState);
            player.useSkill = false;
        }
    }
}
