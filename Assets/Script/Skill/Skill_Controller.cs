using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Skill_Controller : MonoBehaviour
{
    protected Animator animator;
    protected Player player;
    protected CapsuleCollider2D cd => GetComponent<CapsuleCollider2D>();

    protected List<CharacterStats> characterStats;
    protected Collider2D[] colliders;

    protected bool CanDamage;

    public GameObject markPrefab;
    protected bool CanContinuous;
    protected MarkType mark;
   
    protected float magicdamage;
    protected float probability;
    protected float poisedamage;
    protected Vector2 knockPower;

    public SkillLevelType skillType;
    protected AttributeType magicType;
    protected float totalMagicDamage;
    protected float moveSpeed;
    protected int facingDir;

    public virtual void SetupSkill(float _magicdamage, float _probability, float _poisedamage,Vector2 _knockPower, bool _CanContinuous, float _moveSpeed, Player _player, AttributeType _musicType, SkillLevelType _skillType)
    {
        magicdamage = _magicdamage;
        probability = _probability;
        poisedamage = _poisedamage;
        knockPower = _knockPower;
        CanContinuous = _CanContinuous;
        moveSpeed = _moveSpeed;
        player = _player;
        magicType = _musicType;
        skillType = _skillType;
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        characterStats = new List<CharacterStats>();

        totalMagicDamage = magicdamage;
        facingDir = player.facingDir;
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
        colliders = Physics2D.OverlapCapsuleAll(new Vector2(cd.bounds.center.x + cd.offset.x, cd.bounds.center.y + cd.offset.y), cd.size, cd.direction, 0);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if(CanContinuous || !characterStats.Contains(hit.GetComponent<EnemyStat>()))
                {
                    hit.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability, magicType);
                    characterStats.Add(hit.GetComponent<EnemyStat>());
                }

                if(mark != MarkType.None && hit.GetComponent<EnemyStat>().mark != mark)
                {
                    if (hit.GetComponent<EnemyStat>().progress == 0)
                        Instantiate(markPrefab, new Vector3(hit.transform.position.x, hit.transform.position.y + 1, 0), Quaternion.identity, hit.transform);

                    hit.GetComponent<EnemyStat>().progress += 25;

                    if (hit.GetComponent<EnemyStat>().progress == 100)
                    {
                        hit.GetComponent<EnemyStat>().SetMark(mark);
                        hit.GetComponent<EnemyStat>().progress = 0;
                    }
                }
            }
        }
    }

    protected void CanTakeDamage()
    {
        CanDamage = !CanDamage;
    }

    protected virtual void SelfDestroy() => Destroy(gameObject);

}
