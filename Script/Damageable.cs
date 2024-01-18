using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator animator;
    SpriteRenderer sp;
    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;
    public float flashTime = 0.05f;
    [NonSerialized]
    public float damageRate;
    public bool canBroke;
    public GameObject brockFX;

    private int _curentBroke = 0;

    public int currentBroke
    {
        get
        {
            return _curentBroke;
        }
        set
        {
            _curentBroke = value;
        }
    }


    [SerializeField]
    private int _maxDefence = 100;
    public int MaxDefence
    {
        get
        {
            return _maxDefence;
        }
        set
        {
            _maxDefence = value;
        }
    }
    
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            if (_health <= 0)
            {
                IsAlive = false;
                CharacterEvent.characterDead.Invoke(gameObject);
                Debug.Log("dead");
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
        }
    }

    public bool IsInvincible = false;
    private bool IsFlashing;
    private float timeSinceFlash;
    private bool broken;

    private float timeSinceBroke;
    public float brokeTime = 1f;
    private float timeSinceRecover;
    public float recoverTime = 0.5f;

    public bool IsHit
    {
        get
        {
            return animator.GetBool(AnimationStrings.isHit);
        }
        private set
        {
            animator.SetBool(AnimationStrings.isHit, value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public bool IsBroken
    {
        get
        {
            return animator.GetBool(AnimationStrings.isBroke);
        }
        set
        {
            if(animator.GetBool(AnimationStrings.isBroke) != value && value == true)
            {
                Instantiate(brockFX, gameObject.GetComponent<Transform>().position, brockFX.transform.rotation);
            }
            animator.SetBool(AnimationStrings.isBroke, value);
        }
    }

    private int recover = 1;





    // Start is called before the first frame update



    private void Awake()
    {
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

    }

    public bool Hit(int damage,Vector2 knockback,int brokeRate,bool isPenetrating)
    {
        if(IsAlive && !IsInvincible)
        {
            damageRate = Mathf.Sqrt(damage + 343) - 18;
            int shockRate = (int)(Mathf.Pow(damageRate+1, 2));
            int actualDamage = DamageDefence(damage, isPenetrating);
            Health -= actualDamage;
            IsInvincible = true;
            LockVelocity = true;
            IsHit = true;
            recover = 1;
            ShockValue(brokeRate);

            if (damage > 10)
            {
                AttackSence.Instance.CameraShake(0.025f, (float)damage/200);
            }

            if(!isPenetrating && gameObject.name != "player")
                AttackSence.Instance.HitPause(shockRate);

            damageableHit?.Invoke(damage, knockback);
            //sp.material.SetColor("_FlashColor", new Color(0.7f, 0.2f, 0.2f));
            //sp.material.SetFloat("_FlashAmount", (float)0.7);
            IsFlashing = true;
            CharacterEvent.characterDamaged.Invoke(gameObject, actualDamage);
            return true;
        }
        else
        {
            return false;
        }

    }

    private int DamageDefence(int damage,bool isPenetrating)
    {
        if(!broken && canBroke &&!isPenetrating)
        {
            damage /= 4;
        }
        return damage;
    }

    private void ShockValue(int brokeRate)
    {
        if (canBroke)
        {
            if (!broken)
            {
                currentBroke += brokeRate;
                GetComponentInChildren<BrokeBar>().UpdateBroke();
                if (!(currentBroke < MaxDefence))
                {
                    broken = true;
                    IsBroken = true;
                    CharacterEvent.characterBroken.Invoke(gameObject);
                    currentBroke = 0;
                    AttackSence.Instance.HitPause(10);
                    AttackSence.Instance.CameraShake(0.05f, 0.6f);
                }
            }
            if (IsBroken)
            {
                timeSinceBroke = Mathf.Max(timeSinceBroke - brokeRate / 50, 0);
            }
        }
    }

    public void Heal(int healthRestore)
    {
        if (IsAlive)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            healthRestore = Mathf.Min(healthRestore, maxHeal);
            CharacterEvent.characterHealed.Invoke(gameObject, healthRestore);
            Health += healthRestore;
            //sp.material.SetColor("_FlashColor", new Color(0, 0.7f, 0.6f));
            //sp.material.SetFloat("_FlashAmount", 0.7f);
            IsFlashing = true;
        }
    }

    private void Update()
    {
        if (IsAlive)
        {
            if (IsInvincible)
            {
                if (timeSinceHit > invincibilityTime)
                {
                    IsInvincible = false;
                    timeSinceHit = 0;
                }

                timeSinceHit += Time.deltaTime;
            }

            if (broken)
            {
                if (timeSinceBroke > brokeTime)
                {
                    broken = false;
                    currentBroke = 0;
                    MaxDefence /= 2;
                    timeSinceBroke = 0;
                    IsBroken = false;
                    GetComponentInChildren<BrokeBar>().broke.fillAmount = 0;

                }
                timeSinceBroke += Time.deltaTime;
            }

            if (!broken && canBroke && currentBroke > 0)
            {
                if (timeSinceRecover > recoverTime)
                {
                    int maxRecover = currentBroke - 0;
                    currentBroke -= Mathf.Min(maxRecover, recover);
                    recover += recover;
                    timeSinceRecover = 0;
                }
                timeSinceRecover += Time.deltaTime;
            }
        }
        if (IsFlashing)
        {
            if (timeSinceFlash > flashTime)
            {
                IsFlashing = false;
                sp.material.SetFloat("_FlashAmount", 0);
                timeSinceFlash = 0;
            }
            timeSinceFlash += Time.deltaTime;
        }

    }




}