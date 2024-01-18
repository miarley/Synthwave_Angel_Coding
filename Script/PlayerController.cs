using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float attackSpeed = 5f;
    public float slideSpeed = 30f;
    public float reloadingSpeed = 10f;
    public float healSettingSpeed = 6f;
    public float airmoveSpeed = 3f;
    public float jumpImpulse = 10f;
    public float moveSpeed = 15f;
    public float switchRate = 0.5f;
    public float slideTime = 0.3f;
    public float reloadingTime = 2f;
    public float healSettingTime = 0.5f;
    public float airSlideTime = 30f;
    public int airSlideCount = 1;
    private float timeSinceSlide = 0;
    private float timeSinceReload = 0;
    private float timeAfterAttack = 0;
    public float stopRate = 0.2f;
    public int attackExpect = 1;
    public float airSlideSpeed = 25f;
    public float comboTime = 3f;
    private float currentDirection;
    private float originRb;
    public bool isAttacking = false;
    public bool IsFiring = false;
    public UnityEvent onShoot;
    ClosestDetection closest;
    public Vector3 shootingTarget;
    public float shootingAngle;
    private GameObject playerPanel;
    Transform[] ChildrenTrans = new Transform[20];
    private float maxAmmo;
    private int maxAirSlide;
    private float timeSinceSetHeal;
    Vector2 moveInput;


    //判断当前速度
    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (touchingDirection.IsGrounded)
                {
                    if (IsReloading && IsMoving)
                    {
                        return reloadingSpeed;
                    }
                    else if (HealSetting && IsMoving)
                    {
                        return healSettingSpeed;
                    }
                    else if (IsMoving)
                    {

                        return moveSpeed;

                    }
                    else
                    {
                        return 0;
                    }


                }
                else if(!touchingDirection.IsOnWall)
                {
                    return airmoveSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

    }


    [SerializeField]
    private bool _IsMoving = false;
    [SerializeField]
    private bool _IsSliding = false;

    private bool _IsReloading = false;

    private int _AttackCount = 1;

    public bool _IsFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _IsFacingRight;
        }
        private set
        {
            if (_IsFacingRight != value)
            {
                for(int i = 1; i < ChildrenTrans.Length; i++)
                {
                    ChildrenTrans[i].localScale = new Vector3(-ChildrenTrans[i].localScale.x, ChildrenTrans[i].localScale.y, ChildrenTrans[i].localScale.z);
                }
                sp.flipX = !value;
            }

            _IsFacingRight = value;
        }
    }



    Rigidbody2D rb;
    Animator animator;
    TouchingDirection touchingDirection;
    Damageable damageable;
    SpriteRenderer sp;
    public GameObject healZone;
    ConditionCanvas canvas;



    //跑步状态动画
    public bool IsMoving
    {
        get
        {
            return _IsMoving;
        }
        private set
        {
            _IsMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }



    public float slidePosition;
    Vector2 saveMoveInput;
    [SerializeField]
    private int preAttack;
    private Color originalColor;





    //滑行状态动画
    public bool IsSliding
    {
        get
        {
            return _IsSliding;
        }
        set
        {
            if (_IsSliding && !value)
            {
                sp.material.SetColor("_FlashColor", originalColor);
                sp.material.SetFloat("_FlashAmount", 0);
                timeSinceSlide = 0;
                damageable.IsInvincible = false;
                rb.gravityScale = originRb;
            }

            _IsSliding = value;
            animator.SetBool(AnimationStrings.isSliding, value);
        }
    }

    [SerializeField]
    private int _healCount = 3;

    public int HealCount
    {
        get
        {
            return _healCount;
        }
        set
        {
            if (_healCount != value)
            {
                playerPanel.GetComponent<RecoverBanner>().UpdateRecover(value);
            }
            _healCount = value;
        }
    }


    public bool IsReloading
    {
        get
        {
            return _IsReloading;
        }
        set
        {
            if(_IsReloading != value)
            {

                canvas.OnUpdateReloading(value);
            }
            
            _IsReloading = value;
            animator.SetBool(AnimationStrings.isReloading, value);
        }
    }

    private bool _healSetting;

    public bool HealSetting
    {
        get
        {
            return _healSetting;
        }
        set
        {
            _healSetting = value;
            animator.SetBool(AnimationStrings.healSetting, value);
        }
    }


    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public int AttackCount
    {
        get
        {
            return _AttackCount;
        }
        set
        {
            _AttackCount = value;
            animator.SetFloat(AnimationStrings.attackCount, value);
            animator.SetTrigger(AnimationStrings.attackTrigger);

        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    [SerializeField]
    private float _ammo = 7;
    public float Ammo
    {
        get
        {
            return _ammo;
        }
        set
        {
            if (value != _ammo)
            {
                playerPanel.GetComponent<PlayerHP>().AmmoCount(value);
                _ammo = value;
            }

            animator.SetFloat(AnimationStrings.ammo, value);

        }
    }



    //当代码唤醒时
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
        originRb = rb.gravityScale;
        closest = GetComponentInChildren<ClosestDetection>();
        sp = GetComponent<SpriteRenderer>();
        //originalColor = sp.material.GetColor("_FlashColor");
        playerPanel = GameObject.Find("Canvas");
        canvas = GetComponentInChildren<ConditionCanvas>();
        maxAirSlide = airSlideCount;
        maxAmmo = Ammo;
        ChildrenTrans = GetComponentsInChildren<Transform>();
    }


    //每次刷新时
    private void FixedUpdate()
    {
        if (!damageable.IsHit)
        {

            //滑行
            if (IsSliding)
            {
                rb.transform.position = new Vector2(rb.position.x, slidePosition);
                damageable.IsInvincible = true;
                SetFacingDirection(new Vector2(currentDirection, 0));

                if (touchingDirection.IsGrounded)
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, slideSpeed * currentDirection, switchRate), rb.velocity.y);
                }
                //空中冲刺
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, airSlideSpeed * currentDirection, switchRate), rb.velocity.y);
                    rb.gravityScale = 0;
                }



                if (touchingDirection.IsGrounded)
                {
                    if (timeSinceSlide > slideTime)
                    {
                        IsSliding = false;
                    }

                }
                else
                {
                    if (timeSinceSlide > airSlideTime)
                    {
                        IsSliding = false;
                    }
                }


                timeSinceSlide += Time.deltaTime;
            }

            //攻击位移

            else if (isAttacking && !IsFiring)
            {
                SetFacingDirection(new Vector2(currentDirection, 0));
                rb.velocity = new Vector2(attackSpeed * currentDirection * Mathf.Pow(Mathf.Lerp(1, 0, stopRate), 3), rb.velocity.y);
            }
            //射击位移
            else if (IsFiring && !isAttacking)
            {
                SetFacingDirection(new Vector2(currentDirection, 0));
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, moveInput.x * CurrentSpeed, switchRate), rb.velocity.y);

        }

        //y轴速度
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        //段数清零
        if (!isAttacking)
        {


            if (timeAfterAttack > comboTime)
            {

                timeAfterAttack = 0;
                attackExpect = 1;

            }
            //段数计时
            timeAfterAttack += Time.deltaTime;

        }
        //段数
        if (attackExpect > 3)
        {
            attackExpect = 1;
        }

        //preattack
        if (!IsSliding && touchingDirection.IsGrounded && !damageable.LockVelocity)
        {
            if (!isAttacking && preAttack > 0)
            {
                if (moveInput.x != 0)
                {
                    currentDirection = moveInput.x;
                }
                else if (IsFacingRight)
                {
                    currentDirection = 1;
                }
                else
                {
                    currentDirection = -1;
                }

                //归零地面攻击
                preAttack--;
                AttackCount = attackExpect;
                timeAfterAttack = 0;
            }
        }




        //动作结束时翻转
        if (!IsSliding && !damageable.LockVelocity && !isAttacking && !IsFiring)
        {
            if (rb.velocity.x * saveMoveInput.x > 0)
            {
                SetFacingDirection(saveMoveInput);
                saveMoveInput = Vector2.zero;
            }
        }


        //装弹
        if (IsReloading)
        {
            timeSinceReload += Time.deltaTime;
            if (timeSinceReload > reloadingTime)
            {
                IsReloading = false;
                Ammo = maxAmmo;
                timeSinceReload = 0;
            }
        }

        //落地重置slide
        if (touchingDirection.IsGrounded && airSlideCount < maxAirSlide)
        {
            airSlideCount = maxAirSlide;
        }

    }

    //当按下move指令时
    public void OnMove(InputAction.CallbackContext context)
    {


        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            if (!IsSliding && !damageable.LockVelocity && !isAttacking && !IsFiring)
            {


                SetFacingDirection(moveInput);

            }
            else
            {
                saveMoveInput = moveInput;
            }

        }
        else
        {
            IsMoving = false;
        }



    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //当方向向右
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            //当方向向左
            IsFacingRight = false;
        }
    }

    //回复滑行指令时
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (damageable.IsAlive)
        {
            if (context.started && !IsSliding && !damageable.LockVelocity && !HealSetting)
            {

                if (!touchingDirection.IsGrounded && airSlideCount > 0 && !touchingDirection.IsOnWall)
                {
                    SetCurrentDirection();

                    slidePosition = rb.position.y;
                    IsSliding = true;
                    airSlideCount--;
                }
                else if (touchingDirection.IsGrounded)
                {
                    SetCurrentDirection();

                    slidePosition = rb.position.y;
                    IsSliding = true;
                }

                //打断填弹
                IsReloading = false;
            }



        }

    }

    //回复跳跃指令时
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && !damageable.LockVelocity && !HealSetting)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            IsSliding = false;

            //打断填弹
            IsReloading = false;
        }
    }

    //回复攻击指令时
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsSliding && touchingDirection.IsGrounded && !damageable.LockVelocity && !HealSetting)
        {
            if (context.started && !isAttacking && preAttack < 1)
            {
                SetCurrentDirection();


                //归零地面攻击

                playerPanel.GetComponent<PlayerHP>().ModeChange(0);

                AttackCount = attackExpect;
                timeAfterAttack = 0;

                //打断填弹
                IsReloading = false;
            }
            else if (context.started && isAttacking && preAttack < 1)
            {
                preAttack++;
            }

        }


    }

    //回复射击
    public void OnFire(InputAction.CallbackContext context)
    {
        if (!IsSliding && touchingDirection.IsGrounded && !damageable.LockVelocity &&!HealSetting)
        {
            if (context.started && !IsFiring)
            {
                if (Ammo > 0)
                {
                    SetCurrentDirection();

                    IsFiring = true;

                    playerPanel.GetComponent<PlayerHP>().ModeChange(1);

                    animator.SetTrigger(AnimationStrings.isFiring);

                    onShoot.Invoke();

                    FindTargget();

                    Ammo--;
                }
                else
                {
                    IsReloading = true;


                }


            }
        }


    }



    private void FindTargget()
    {
        if (closest.closestTarget != null)
        {
            shootingTarget = closest.closestTarget.position + new Vector3(0, UnityEngine.Random.Range(-10, 10) / 7, 0);
            Vector3 targetDir = shootingTarget - transform.position;
            float angle = Vector3.Angle(new Vector3(currentDirection, 0, 0), targetDir);
            if (shootingTarget.y < transform.position.y)
            {
                angle *= (-1);

            }
            shootingAngle = angle;

            if (angle > 10)
            {
                animator.SetBool(AnimationStrings.fireUpon, true);
            }
            else
                animator.SetBool(AnimationStrings.fireUpon, false);
        }
        else
        {
            animator.SetBool(AnimationStrings.fireUpon, false);
            shootingTarget = Vector3.zero;
            shootingAngle = 0;

        }
    }

    private void SetCurrentDirection()
    {
        if (moveInput.x != 0)
        {
            currentDirection = moveInput.x;
        }
        else if (IsFacingRight)
        {
            currentDirection = 1;
        }
        else
        {
            currentDirection = -1;
        }
        StartCoroutine(DelayInput());
    }

    IEnumerator DelayInput()
    {
        float timer=0;
        while (timer < 0.1f)
        {
             if (moveInput.x>0)
            {
                currentDirection = 1;
            }
            else if(moveInput.x<0)
            {
                currentDirection = -1;
            }
            timer += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }


    //被攻击时
    public void OnHit(int damage, Vector2 knockback)
    {
        if (damageable.IsHit)
        {
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
            IsSliding = false;
            playerPanel.GetComponent<PlayerHP>().UpdateHp();
        }

    }

    //被治疗时
    public void OnHeal(int restoreAmount)
    {

        damageable.Heal(restoreAmount);
        playerPanel.GetComponent<PlayerHP>().UpdateHp();

    }

    //回复治疗
    public void OnHealSet(InputAction.CallbackContext context)
    {
        if (context.started && HealCount > 0 && !HealSetting)
        {
            HealSetting = true;
            canvas.updateAskingHeal();
            timeSinceSetHeal = 0;
            StartCoroutine(HealIntonate());
            HealCount--;

        }
    }

    IEnumerator HealIntonate()
    {
        while (timeSinceSetHeal < healSettingTime)
        {
            timeSinceSetHeal += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Instantiate(healZone, transform.position, healZone.transform.rotation);
        HealSetting = false;
    }

    public void OnCliff()
    {
        if (IsSliding)
        {
            IsSliding = false;
        }
    }
}
