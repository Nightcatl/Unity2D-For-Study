using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skill_Controller : MonoBehaviour
{
    protected Animator animator;

    protected SkillData skillData;

    protected CapsuleCollider2D cd => GetComponent<CapsuleCollider2D>();

    protected AttributeType attributeType;

    protected bool CanDamage;
    protected bool IsHit;

    public GameObject markPrefab;
    protected bool CanContinuous;
    protected MarkType mark;

    protected float magicdamage;
    protected float probability;
    protected float poisedamage;
    protected Vector2 knockPower;

    protected float totalMagicDamage;
    protected int facingDir;

    public virtual void SetupSkill(SkillData _skilldata)
    {
        skillData = _skilldata;
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        totalMagicDamage = magicdamage;
    }

    protected virtual void Update()
    {
        if (CanDamage)
            AnimationDamageEvent();
    }

    protected virtual void SetVector(Rigidbody2D rb, Vector2 _moveSpeed)
    {
        rb.velocity = new Vector2(_moveSpeed.x * facingDir, _moveSpeed.y);
    }

    public virtual void AnimationDamageEvent()
    {
        if (IsHit && !CanContinuous)
            return;

        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(new Vector2(cd.bounds.center.x + cd.offset.x, cd.bounds.center.y + cd.offset.y), cd.size, cd.direction, 0);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (attributeType != AttributeType.None)
                    hit.GetComponent<PlayerStat>().TakeMagicDamage(skillData.Damage, skillData.KnockPower, skillData.Poisedamage, skillData.Probability, attributeType);
                else
                    hit.GetComponent<PlayerStat>().TakeDamage(skillData.Damage, skillData.KnockPower, skillData.Poisedamage, facingDir);
                IsHit = true;
            }
        }
    }

    protected void CanTakeDamage()
    {
        CanDamage = !CanDamage;
    }

    protected virtual void SelfDestroy() => Destroy(gameObject);
}
