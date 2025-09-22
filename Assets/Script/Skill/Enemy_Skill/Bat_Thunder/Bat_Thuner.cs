using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Thuner : Enemy_Skill_Controller
{
    private Rigidbody2D rb;

    private float timer;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        attributeType = AttributeType.Thunder;

        timer = 1f;
    }

    protected override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            animator.SetBool("Hit", true);

            SetVector(rb, Vector2.zero);

        }else
        {
            CloseToPlayer();
        }
    }

    private void CloseToPlayer()
    {
        float X = PlayerManager.instance.player.transform.position.x;

        if (X > transform.position.x)
        {
            facingDir = 1;
        } 
        else
        {
            facingDir = -1;
        }
        SetVector(rb, new Vector2(skillData.MoveSpeed, 0));
    }
            
}
