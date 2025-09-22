using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider_WaterTraps : MonoBehaviour
{
    private Animator animator;
    private WaterTraps_Skill_Controller waterTraps;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        waterTraps = GetComponentInParent<WaterTraps_Skill_Controller>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Check") && !collision.CompareTag("Ground"))
        {
            if(waterTraps.skillType == SkillLevelType.Right)
            {
                collision.GetComponent<Enemy>().FreezeTimeFor(2f);
            }

            animator.SetBool("IsHit", true);
            waterTraps.isHit = true;
        }
    }
}
