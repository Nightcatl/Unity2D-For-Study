using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    public CharacterStats stat;

    public EntityFX fx;

    #endregion

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatisGround;

    [HideInInspector] public System.Action OnKnocked;
    [HideInInspector] public System.Action onFlipped;

    [Header("Knockback info")]
    public Vector2[] konckbackPower;
    public float knockbackDrag;
    public float knockbackDuration;
    [HideInInspector] public bool isKnockback;

    [Header("Move info")]
    public float moveSpeed;
    [HideInInspector] public float defaultMoveSpeed;
    
    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; protected set; } = true;
    [HideInInspector] public int hitDir;

    [HideInInspector] public bool IsSuperArmor;
    [HideInInspector] public bool IsFreeze;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool IsInvincible;
    [HideInInspector] public bool IsBreak;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        stat = GetComponent<CharacterStats>();
    }

    protected virtual void Start()
    {
        stat.onHealthChanged += Dead;
        stat.onTenacityChanged += Break;
        defaultMoveSpeed = moveSpeed;
    }

    protected virtual void Update()
    {
        if (IsFreeze)
            return;
    }

    public void SetCollide(int layer1,int layer2, bool IsCollide)
    {
        Physics2D.IgnoreLayerCollision(layer1, layer2, IsCollide);
    }

    public void StartFx(int particlesIndex, Vector2 position)
    {
        fx.StartParticles(particlesIndex, position);
    }

    public void DamageImpack(Vector2 _konckbackPower)
    {
        if (!IsSuperArmor && _konckbackPower != Vector2.zero)
        {
            if (OnKnocked != null)
                OnKnocked();

            StartCoroutine(HitKonckback(_konckbackPower));
        }
    }

    protected virtual IEnumerator HitKonckback(Vector2 _konckbackPower)
    {
        isKnockback = true;

        rb.velocity = new Vector2(_konckbackPower.x * knockbackDrag, _konckbackPower.y * knockbackDrag);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;

        SetZeroVelocity();
    }

    #region StateChange
    protected virtual void Dead()
    {
        if(stat.currentHealth <= 0)
            IsDead = true;
    }

    protected virtual void Break()
    {
        if(stat.currentTenacity <= 0)
            IsBreak = true;
    }

    public void SetInvincibility()
    {
        IsInvincible = true;

        SetCollide(6, 9, IsInvincible);
    }

    public void RemoveInvincibility()
    {
        IsInvincible = false;

        SetCollide(6, 9, IsInvincible);
    }
    #endregion

    #region Collsion
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatisGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatisGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public void SetdefaultFacing(int _facingDir)
    {
        facingDir = _facingDir;
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        if (x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnockback)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockback)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetVelocityForAttack(float _xVelocity, float _yVelocity)
    {
        if (isKnockback)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
    }
    #endregion

    #region Freeze
    public void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        IsFreeze = true;

        yield return new WaitForSeconds(_seconds);

        IsFreeze = false;

        FreezeTime(false);
    }
    #endregion
}
