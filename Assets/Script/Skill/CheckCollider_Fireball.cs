using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider_Fireball : MonoBehaviour
{
    private Animator animator;
    private Fireball_Skill_Controller fireball;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        fireball = GetComponentInParent<Fireball_Skill_Controller>();
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player") && !collision.CompareTag("Check"))
        {
            if(fireball.skillType == SkillLevelType.Right && collision.CompareTag("Enemy"))
            {
                FireMark fireMark = Instantiate(fireball.markPrefab, new Vector3(collision.transform.position.x, collision.transform.position.y + 1, 0), Quaternion.identity, collision.transform).GetComponent<FireMark>();
                fireMark.SetUpMark(true); 
            }

            animator.SetBool("IsHit", true);
            fireball.isHit = true;
            gameObject.SetActive(false);
        }
    }*/
}
