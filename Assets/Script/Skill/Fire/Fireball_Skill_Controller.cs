using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Skill_Controller : Skill_Controller
{
    private Rigidbody2D rb;
    private Vector2 finalMoveSpeed;

    private float ExitTime;

    public bool isHit;

    public override void SetupSkill(float _magicdamage, float _probability, float _poisedamage, Vector2 _knockPower, bool _CanContinuous, float _moveSpeed, Player _player, AttributeType _musicType, SkillLevelType _skillType)
    {
        base.SetupSkill(_magicdamage, _probability, _poisedamage, _knockPower, _CanContinuous, _moveSpeed, _player, _musicType, _skillType);
    }

    public void SetUpExitTime(float _ExitTime)
    {
        ExitTime = _ExitTime;
    }

    protected override void Start()
    {
        cd.size = new Vector2(cd.size.x, 0.7f);

        base.Start();
        rb = GetComponent<Rigidbody2D>();

        finalMoveSpeed = new Vector2(moveSpeed * Mathf.Cos(AimSquare_Controller.instance.Z / Mathf.Rad2Deg) * facingDir, moveSpeed * Mathf.Sin(AimSquare_Controller.instance.Z / Mathf.Rad2Deg));

        SetVector(rb, finalMoveSpeed);
    }

    protected override void Update()
    {
        if (isHit)
        {
            cd.size = new Vector2(cd.size.x, 2f);
            AnimationDamageEvent();
        }
            

        ExitTime -= Time.deltaTime;

        if(ExitTime <= 0 && !isHit)
            SelfDestroy();

        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHit && collision.CompareTag("Enemy"))
        {
            if (skillType == SkillLevelType.Right && collision.CompareTag("Enemy"))
            {
                FireMark fireMark = Instantiate(markPrefab, new Vector3(collision.transform.position.x, collision.transform.position.y + 1, 0), Quaternion.identity, collision.transform).GetComponent<FireMark>();
                fireMark.SetUpMark(true);
            }

            isHit = true;
            animator.SetBool("IsHit", true);
        }
    }
}
