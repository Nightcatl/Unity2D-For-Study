using System;
using System.Collections;
using UnityEngine;

public enum MarkType
{
    None,
    Fire,
    BreakFire,
}

public enum StatName
{
    Damge = 0,
    MaxHealth = 1,
    Magicpoint = 2,
    Vitality = 3,
    Strength = 4,
    Intelligence = 5,
    Critical = 6,
    Armor = 7,
    MagicResistance = 8,
    Resistance_Fire = 9,
    Resistance_Water = 10,
    Resistance_Thuner = 11,
    Resistance_Lightning = 12,
    Resistance_Poison = 13,
    Poisedamage = 14,
    StunDamage = 15
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    private Entity entity;

    [Header("Base")]
    public Stat Damage;
    public Stat MaxHealth;
    public Stat Magicpoint;
    public Stat Vitality;

    [Header("Damage")]
    public Stat Strength;
    public Stat Intelligence;
    public Stat Critical;

    [Header("Defense")]
    public Stat Armor;
    public Stat MagicResistance;
    public Stat[] Resistance;

    [Header("Tenacity")]
    public Stat Tenacity;
    public Stat Poisedamage;

    [Space]
    public float currentHealth;
    public float currentTenacity;
    public float currentStunGauge;
    public float currentVitality;
    public float currentMagicpoint;

    public System.Action onTakeDamage;
    public System.Action onHealthChanged;
    public System.Action onVitalityChanged;
    public System.Action onMagicpointChanged;
    public System.Action onMagicpointAmountChanged;
    public System.Action onTenacityChanged;

    [Header("Status Effects")]
    public bool isIgnited;
    public bool isDamp;
    public bool isShock;
    public bool isPalsy;

    [SerializeField] private float ignitedDuration;
    [SerializeField] private float dampDuration;
    [SerializeField] private float shockDuration;
    [SerializeField] private float palsyDuration;
    private float ignitedTimer;
    private float dampTimer;
    private float shockTimer;
    private float palsyTimer;

    public int progress;

    public MarkType mark;

    protected DateTime lastGetDamageTime {get; private set;}

    private void Awake()
    {
        currentHealth = GetStatVaule(StatName.MaxHealth);
        currentTenacity = Tenacity.GetValue();
        currentVitality = Vitality.GetValue();
        currentMagicpoint = Magicpoint.GetValue();
        currentTenacity = Tenacity.GetValue();


        fx = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(isIgnited)
        {
            ignitedTimer -= Time.deltaTime;
            if(ignitedTimer <= 0)
                StopIgnite();
        }

        if(isDamp)
        {
            dampTimer -= Time.deltaTime;
            if(dampTimer <= 0)
                StopDamp();
        }

        if(isShock)
        {
            shockTimer -= Time.deltaTime;
            if(shockTimer <= 0)
                StopShock();
        }

        if(isPalsy)
        {
            palsyTimer -= Time.deltaTime;
            if(palsyTimer <= 0)
                StopPalsy();
        }
    }

    public virtual void IncreaseStatModBy(int _modifier, float _duration, Stat _statToModifier)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModifier));
    }

    IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModifier)
    {
        _statToModifier.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModifier.RemoveModifier(_modifier);
    }

    #region Stat
    public virtual void IncreseVitalityBy(int _amount)
    {
        currentVitality += _amount;

        if(currentVitality > MaxHealth.GetValue())
            currentVitality = MaxHealth.GetValue();

        if (onVitalityChanged != null)
            onVitalityChanged();
    }

    public virtual void DecreaseVitalityBy(int _amount)
    {
        currentVitality -= _amount;

        if(onVitalityChanged!= null)
            onVitalityChanged();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetStatVaule(StatName.MaxHealth))
            currentHealth = GetStatVaule(StatName.MaxHealth);

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(float _damage)
    {
        currentHealth -= _damage;

        if(onHealthChanged!= null) 
            onHealthChanged();
    }

    public virtual void IncreseMagicPoint(float _amount)
    {
        currentMagicpoint += _amount;

        if(onMagicpointChanged!= null) 
            onMagicpointChanged();
    }

    public virtual void DecreseMagicPoint(float _amount)
    {
        currentMagicpoint -= _amount;

        if (onMagicpointChanged != null)
            onMagicpointChanged();
    }

    public virtual void IncreseTenacity(int _amount)
    {
        currentTenacity += _amount;

        if(onTenacityChanged != null)
            onTenacityChanged();
    }

    public virtual void DecreseTenacityBy(int _amount)
    {
        currentTenacity -= _amount;

        if(onTenacityChanged != null)
            onTenacityChanged();
    }
    
    public int GetStatVaule(StatName type)
    {
        switch(type)
        {
            case StatName.Damge:
                return Damage.GetValue();
            case StatName.MaxHealth:
                return MaxHealth.GetValue();
            case StatName.Magicpoint:
                return Magicpoint.GetValue();
            case StatName.Vitality:
                return Vitality.GetValue();
            case StatName.Strength:
                return Strength.GetValue();
            case StatName.Intelligence:
                return Intelligence.GetValue();
            case StatName.Critical:
                return Critical.GetValue();
            case StatName.Armor:
                return Armor.GetValue();
            case StatName.MagicResistance:
                return MagicResistance.GetValue();
            case StatName.Resistance_Fire:
                return Resistance[0].GetValue();
            case StatName.Resistance_Water:
                return Resistance[1].GetValue();
            case StatName.Resistance_Lightning:
                return Resistance[2].GetValue();
            case StatName.Resistance_Poison:
                return Resistance[3].GetValue();
            case StatName.Poisedamage:
                return Poisedamage.GetValue();
            default:
                return 0;
        }
    }

    public virtual void SetStatVaule(StatName type, int Num)
    {
        switch (type)
        {
            case StatName.Damge:
                Damage.SetDefaultValue(Num);
                break;
            case StatName.MaxHealth:
                MaxHealth.SetDefaultValue(Num);
                break;
            case StatName.Magicpoint:
                Magicpoint.SetDefaultValue(Num);
                break;
            case StatName.Vitality:
                Vitality.SetDefaultValue(Num);
                break;
            case StatName.Strength:
                Strength.SetDefaultValue(Num);
                break;
            case StatName.Intelligence:
                Intelligence.SetDefaultValue(Num);
                break;
            case StatName.Critical:
                Critical.SetDefaultValue(Num);
                break;
            case StatName.Armor:
                Armor.SetDefaultValue(Num);
                break;
            case StatName.MagicResistance:
                MagicResistance.SetDefaultValue(Num);
                break;
            case StatName.Resistance_Fire:
                Resistance[0].SetDefaultValue(Num);
                break;
            case StatName.Resistance_Water:
                Resistance[1].SetDefaultValue(Num);
                break;
            case StatName.Resistance_Lightning:
                Resistance[2].SetDefaultValue(Num);
                break;
            case StatName.Resistance_Poison:
                Resistance[3].SetDefaultValue(Num);
                break;
            case StatName.Poisedamage:
                Poisedamage.SetDefaultValue(Num);
                break;
        }
    }
    #endregion

    #region Damage
    public virtual void TakeDamage(float _damage, Vector2 knockPower, float _poisedamage,int hitDir)
    {
        if (entity.IsInvincible)
            return;

        CheckHitInterval();

        entity.hitDir = hitDir;

        fx.PlayHitFX();

        if (onTakeDamage != null)
            onTakeDamage();

        AttackHandle(knockPower, _poisedamage, hitDir);

        float totalDamage = _damage - (Armor.GetValue() * .5f);

        DecreaseHealthBy(totalDamage);
    }

    public virtual void TakeMagicDamage(float _magicdamage, Vector2 knockPower, float _poisedamage, float _probability, AttributeType _magicType)
    {
        if (entity.IsInvincible)
            return;

        fx.PlayHitFX();

        CheckHitInterval();

        if (onTakeDamage != null)
            onTakeDamage();

        AttackHandle(knockPower, _poisedamage, entity.facingDir);

        float totalMagicdamage = CalculateMgaicDamage(_magicdamage, _magicType);

        DecreaseHealthBy(_magicdamage);

        if (_probability > UnityEngine.Random.Range(0, 100))
        {
            switch (_magicType)
            {
                case AttributeType.Fire:
                    if (!isIgnited)
                    {
                        Ignite();
                        isIgnited = true;
                    }
                    else
                    {
                        ignitedTimer = ignitedDuration;
                    }
                    break;
                case AttributeType.Water:
                    if (!isDamp)
                    {
                        Damp();
                        isDamp = true;
                    }
                    else
                    {
                        dampTimer = dampDuration;
                    }
                    break;
                case AttributeType.Thunder:
                    if (!isShock)
                    {
                        Shock();
                        isShock = true;
                    }
                    else
                    {
                        isPalsy = true;
                        Palsy();
                        StopShock();
                        Debug.Log(2);
                    }
                    break;
            }
        }
    }

    private float CalculateMgaicDamage(float _magicdamage, AttributeType _magicType)
    {
        float totalMagicdamage = _magicdamage - (MagicResistance.GetValue() * .4f);

        switch (_magicType)
        {
            case AttributeType.Fire:
                totalMagicdamage -= (Resistance[0].GetValue() * .3f);
                break;
            case AttributeType.Water:
                totalMagicdamage -= (Resistance[1].GetValue() * .3f);
                break;
            case AttributeType.Thunder:
                totalMagicdamage -= (Resistance[2].GetValue() * .3f);
                break;
        }

        return totalMagicdamage;
    }

    private void AttackHandle(Vector2 knockPower, float _poisedamage, int hitDir)
    {
        if (!entity.IsBreak && !entity.isKnockback)
        {
            DecreseTenacityBy((int)_poisedamage);

            if (!entity.isKnockback)
                entity.DamageImpack(new Vector2(knockPower.x * hitDir, knockPower.y));
        }
    }

    private void CheckHitInterval()
    {
        StartCoroutine(CheckHitIntervalFor());
    }

    private IEnumerator CheckHitIntervalFor()
    {
        entity.IsInvincible = true;

        yield return new WaitForSeconds(.2f);
        
        entity.IsInvincible = false;
    }

    public virtual void BuffDamage(float _damage, float _poisedamage, string _magicType)
    {
        if (entity.IsInvincible)
        {
            return;
        }

        float totalMagicdamage = _damage;

        switch (_magicType)
        {
            case "Fire":
                totalMagicdamage -= (Resistance[0].GetValue() * .7f);
                break;
        }

        DecreaseHealthBy(totalMagicdamage);

        if (!entity.IsBreak)
            DecreseTenacityBy((int)_poisedamage);
    }
    #endregion

    #region Buff

    public void Ignite()
    {
        ignitedTimer = ignitedDuration;
        fx.IgniteFxFor();
        InvokeRepeating("IgniteDamage", 0, 0.3f);
    }

    private void IgniteDamage()
    {
        BuffDamage(1, 0, "Fire");
    }

    private void StopIgnite()
    {
        fx.CancelColorChange();
        CancelInvoke("IgniteDamage");
        isIgnited = false;
    }

    public void Damp()
    {
        dampTimer = dampDuration;
        fx.DampFxFor();
        Slowdown(.7f);
    }

    private void StopDamp()
    {
        isDamp = false;
        fx.CancelColorChange();
        ReturnDefaultMoveSpeed();
    }

    private void Shock()
    {
        shockTimer = shockDuration;
        fx.ShockFxFor();
    }

    private void StopShock()
    {
        isShock = false;
        fx.CancelColorChange();
    }

    private void Palsy()
    {
        palsyTimer = palsyDuration;
        entity.FreezeTimeFor(palsyDuration);
        fx.ShockFxFor();
    }

    private void StopPalsy()
    {
        isPalsy = false;
        fx.CancelColorChange();
    }

    public virtual void Slowdown(float _num)
    {
        entity.moveSpeed *= _num;
    }

    public virtual void ReturnDefaultMoveSpeed()
    {
        entity.moveSpeed = entity.defaultMoveSpeed;
    }

    #endregion

    public void SetMark(MarkType _mark)
    {
        if (mark == MarkType.None || _mark == MarkType.None)
        {
            mark = _mark;
        }else
        {
            switch(mark)
            {
                case MarkType.Fire:
                    GetComponent<FireMark>().Explosion();
                    mark = _mark;
                    break;
            }
        }
    }
}
