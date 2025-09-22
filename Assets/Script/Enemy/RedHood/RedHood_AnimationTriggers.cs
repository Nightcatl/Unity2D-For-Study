using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHood_AnimationTriggers : Enemy_AnimationTriggers
{
    protected Enemy_RedHood enemy_Red => GetComponentInParent<Enemy_RedHood>();

    private void Archery()
    {
        if(enemy_Red.archeryType == 0)
        { 
            GameObject arrow = Instantiate(enemy_Red.arrowPrefab, new Vector2(enemy_Red.transform.position.x + -1, enemy_Red.transform.position.y + -0.7f), Quaternion.identity);

            if (enemy.facingDir == 1)
                arrow.transform.Rotate(0, 180, 0);

            RedHood_Arrow redHood_Arrow = arrow.GetComponent<RedHood_Arrow>();
            redHood_Arrow.SetFacingDir(enemy_Red.facingDir);
            redHood_Arrow.SetupSkill(EnemySkillManager.instance.redHood_Arrow);
        }
        else
        {
            float angle = 0;

            for (int i = 0; i < 3; i++)
            {
                GameObject arrow = Instantiate(enemy_Red.arrowPrefab, new Vector2(enemy_Red.transform.position.x + -1, enemy_Red.transform.position.y + -0.7f), Quaternion.identity);
                

                if(enemy.facingDir == 1)
                    arrow.transform.Rotate(0, 180,  - angle);
                else
                    arrow.transform.Rotate(0, 0, -angle);

                RedHood_Arrow redHood_Arrow = arrow.GetComponent<RedHood_Arrow>();
                redHood_Arrow.SetFacingDir(enemy_Red.facingDir);
                redHood_Arrow.SetupSkill(EnemySkillManager.instance.redHood_Arrow);
                redHood_Arrow.SetAngle(angle * Mathf.Deg2Rad);

                angle += 20;
            }
        }
    }

    protected override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
