using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    [Space]
    [SerializeField] private List<ItemData_Material> dropItem;

    [Header("State Info")]
    public float idleTime;
    public float moveTime;
    public float seekTime;
    public float dashTime;
    public float battleDuration;


    [Space]
    public LayerMask whatIsPlayer;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    public float maxAttackCooldown;
    public float minAttackCooldown;
    [HideInInspector] public float lastTimeAttack;
    [HideInInspector] public bool takeDamage;

    [HideInInspector] public bool FindPlayer;
    [HideInInspector] public bool SeekPlayer;
    [HideInInspector] public float SeekTimer;

    public GameObject SeekPlayerPrefab;
    public GameObject CollideCheckPrefab;

    public TextMeshProUGUI text;

    public EnemyStateMachine stateMachine { get; private set; }

    private Player player;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if(SeekPlayer)
        {
            SeekTimer -= Time.deltaTime;
        }

        if (takeDamage)
            AttackTrigger();
    }

    public virtual void AnimationFinshTrigger() => stateMachine.currentState.AnimationFinshTrigger();

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                int hitDir = facingDir;

                Player player = hit.GetComponent<Player>();

                if (player.isBlock && hitDir == -player.facingDir)
                {
                    player.enemy = transform;
                    player.isBlockSuccess = true;
                    return;
                }

                float Damage = stat.Damage.GetValue() * (1 + stat.Strength.GetValue() * .015f);
                hit.GetComponent<PlayerStat>().TakeDamage(Damage, konckbackPower[0], stat.Poisedamage.GetValue(), hitDir);
            }
        }
    }

    public bool CheckAttackCooldown()
    {
        attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);

        if (Time.time >= lastTimeAttack + attackCooldown)
        {
            lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }

    public bool CheckPlayerFacing()
    {
        if ((player.transform.position.x > transform.position.x && facingRight) || (player.transform.position.x < transform.position.x && !facingRight))
            return true;
        else 
            return false;
    }

    public bool CanAttackPlayer(Vector2 Attackposition, float Attackradius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Attackposition, Attackradius);
        
        foreach(var collider in colliders)
        {
            if(collider.GetComponent<Player>() != null)
            {
                return true;
            }
        }

        return false;
    }

    public Player CanPickUp()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(4,4), 0); 

        if(colliders.Length > 0 )
        {
            foreach(var collider in colliders)
            {
                if(collider.GetComponent<Player>() != null)
                    return collider.GetComponent<Player>();
            }
        }

        return null;
    }

    public void PickUp()
    {
        foreach(var item in dropItem)
        {
            Inventory.instance.AddStash(item);
        }

        player.Body.Remove(this);
        Destroy(gameObject);
    }

    protected override void Dead()
    {
        base.Dead();
    }

    protected override void Break()
    {
        base.Break();
    }

    public virtual void StartToAttack()
    {

    }
}
