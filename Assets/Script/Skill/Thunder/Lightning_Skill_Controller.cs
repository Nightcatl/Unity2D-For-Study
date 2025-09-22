using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lightning_Skill_Controller : Skill_Controller
{
    [SerializeField] private GameObject lightningPrefab;

    private Rigidbody2D rb;
    [SerializeField] private GameObject hitTarget;
    [SerializeField] private List<GameObject> lastTarget;

    private int Hit = 2;

    private int CanSplit;

    private Vector2 finalDir;
    private Vector2 lastDir;
    private Vector2 finalMoveSpeed;
    private float Z;

    public override void SetupSkill(float _magicdamage, float _probability, float _poisedamage, Vector2 _knockPower, bool _CanContinuous, float _moveSpeed, Player _player, AttributeType _musicType, SkillLevelType _skillType)
    {
        base.SetupSkill(_magicdamage, _probability, _poisedamage, _knockPower, _CanContinuous, _moveSpeed, _player, _musicType, _skillType);
    }

    public void SetUpTarget(List<GameObject> _lastTarget, GameObject _hitTarget)
    {
        if(_lastTarget.Count > 0)
        {
            foreach (var target in _lastTarget)
            {
                lastTarget.Add(target);
            }
        }
        
        hitTarget = _hitTarget;
    }

    public void SetUpSplit(int _CanSplit)
    {
        CanSplit = _CanSplit;
    }

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        lastTarget = new List<GameObject>();

        if(player.facingDir != 1)
            transform.Rotate(0, 180, 0);

        finalMoveSpeed = new Vector2(moveSpeed, 0);
    }

    protected override void Update()
    {
        base.Update();

        if(lastTarget.Count != 0)
        {
            finalDir = new Vector2(TargetDirection(hitTarget.transform).normalized.x, TargetDirection(hitTarget.transform).normalized.y);

            lastDir = finalDir;

            SquareDirection();

            finalMoveSpeed = new Vector2(moveSpeed * Mathf.Cos(Z / Mathf.Rad2Deg) * facingDir, moveSpeed * Mathf.Sin(Z / Mathf.Rad2Deg));
        }

        if(Hit != 0)
            SetVector(rb, finalMoveSpeed);

        if(skillType == SkillLevelType.Left)
        {
            if (Hit == 0)
            {
                cd.size = new Vector2(3.4f, 3.7f);
            }
            else if (Hit == 1)
            {
                cd.size = new Vector2(3, 3);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Enemy") && hitTarget == null) || collision.gameObject == hitTarget.gameObject)
        {
            if(skillType == SkillLevelType.Right)
            {
                hitTarget = collision.gameObject;

                collision.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability, magicType);

                if(CanSplit > Random.Range(0,100))
                {
                    GameObject closetEnemy = FindClosetEnemy();

                    if (closetEnemy == null)
                        Debug.Log("Not Enemy");

                    if(closetEnemy != null)
                    {
                        lastTarget.Add(hitTarget);

                        GameObject lightning = Instantiate(lightningPrefab, hitTarget.transform.position, Quaternion.identity);

                        Lightning_Skill_Controller lightning_Skill_Controller = lightning.GetComponent<Lightning_Skill_Controller>();

                        lightning_Skill_Controller.SetupSkill(magicdamage, probability, poisedamage, knockPower, false, moveSpeed, player, magicType, skillType);
                        lightning_Skill_Controller.SetUpTarget(lastTarget, closetEnemy);
                        lightning_Skill_Controller.SetUpSplit(CanSplit - 20);
                    }
                }
                Destroy(gameObject);
            }
            else if(skillType == SkillLevelType.Left)
            {
                Hit--;
                if (Hit == 1)
                {
                    collision.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability * 4, magicType);
                    animator.SetBool("Hit1", true);

                }
                if (Hit == 0)
                {

                    collision.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability * 3, magicType);
                    SetVector(rb, Vector2.zero);
                    animator.SetBool("Hit2", true);
                }
            }
            else
            {
                collision.GetComponent<EnemyStat>().TakeMagicDamage(totalMagicDamage, knockPower, poisedamage, probability, magicType);
                Destroy(gameObject);
            }
        }
    }

    private Vector2 TargetDirection(Transform target)
    {
        return (Vector2)target.position - (Vector2)transform.position;
    }

    private void SquareDirection()
    {
        Z = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;

        Vector3 currentEulerAngles = transform.localEulerAngles;

        currentEulerAngles.z = Z;

        transform.localEulerAngles = currentEulerAngles;

    }

    private GameObject FindClosetEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(hitTarget.transform.position, new Vector2(5, 5), 0);

        GameObject closetEnemy = null;

        float distance = 0;

        if(colliders.Length > 0)
        {
            foreach (var target in colliders)
            {
                if (target.GetComponent<EnemyStat>() != null && !lastTarget.Contains(target.gameObject))
                {
                    if (Vector2.Distance(hitTarget.transform.position, target.transform.position) < distance || distance == 0)
                    {
                        distance = Vector2.Distance(hitTarget.transform.position, target.transform.position);

                        closetEnemy = target.gameObject;
                    }
                }
            }
        }

        return closetEnemy;
    }
}
