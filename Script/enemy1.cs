using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]

public class enemy1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float chaceSpeed = 10f;
    public float walkStopRate = 0.05f;
    public float chaceStopRate = 0.01f;
    public float WaitTime = 6f;
    public float checkTime = 0.5f;
    private float waitTime;
    private float timeSinceAlert;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;
    Animator animator;
    public enum WalkableDirection { Right, Left };
    public DetectionZone attackZone;
    public DetectionZone chaceZone;
    public DetectionZone chaceDetection;
    public Transform patrolL, patrolR;
    private Vector3 currentPatrol, PatrolL, PatrolR;
    private Transform Target;
    private AlertZone alert;
    Damageable damageable;
    public GameObject eventFX;
    Transform[] ChildrenTrans = new Transform[20];
    private SpriteRenderer sp;


    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value && damageable.IsAlive)
            {
                for (int i = 1; i < ChildrenTrans.Length; i++)
                {
                    ChildrenTrans[i].localScale = new Vector3(-ChildrenTrans[i].localScale.x, ChildrenTrans[i].localScale.y, ChildrenTrans[i].localScale.z);
                }
                sp.flipX = value == WalkableDirection.Right ? false : true;

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }


    private bool _attackTarget = false;

    public bool AttackTarget
    {
        get
        {
            return _attackTarget;
        }
        private set
        {

            _attackTarget = value;
            animator.SetBool(AnimationStrings.attackTarget, value);
        }
    }

    private bool _isWaiting = false;

    public bool IsWaiting
    {
        get
        {
            return _isWaiting;
        }
        set
        {
            _isWaiting = value;
            animator.SetBool(AnimationStrings.isWaiting, value);
        }
    }

    private bool _chaceTarget = false;
    private bool lostTarget;
    private bool BeingHit;

    public bool ChaceTarget
    {
        get
        {
            return _chaceTarget;
        }
        set
        {
            if (damageable.IsAlive)
            {
                if (value != _chaceTarget && !value)
                {
                    lostTarget = true;
                }
                if (value && _chaceTarget != value)
                {


                    if (eventFX != null)
                    {
                        Instantiate(eventFX, transform.position + new Vector3(0, 5, 0), eventFX.transform.rotation, this.transform);
                    }
                                        
                }
            }

            _chaceTarget = value;
            animator.SetBool(AnimationStrings.chaceTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
        set
        {
            animator.SetBool(AnimationStrings.canMove, value);
        }
    }

    public float AttackCD
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCD);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCD, value);
        }
    }

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        PatrolL = rb.transform.TransformPoint(patrolL.transform.localPosition);
        PatrolR = rb.transform.TransformPoint(patrolR.transform.localPosition);
        currentPatrol = PatrolR;
        waitTime = WaitTime;
        alert = GetComponentInChildren<AlertZone>();
        sp = GetComponent<SpriteRenderer>();
        ChildrenTrans = GetComponentsInChildren<Transform>();


    }


    void Update()
    {
        if (AttackCD > 0)
        {
            AttackCD -= Time.deltaTime;
        }

        AttackTarget = attackZone.detectedCollider.Count > 0;
        if (chaceDetection.detectedCollider.Count > 0 && chaceZone.currentCollider.GetComponent<Damageable>().IsAlive)
        {
            ChaceTarget = true;
        }
        if (chaceZone.detectedCollider.Count == 0)
        {
            ChaceTarget = false;

        }
        if (damageable.IsHit)
        {
            if (chaceZone.detectedCollider.Count > 0 && chaceZone.currentCollider.GetComponent<Damageable>().IsAlive)
                ChaceTarget = true;

            for (int i = 0; i < alert.detectedCollider.Count; i++)
            {
                alert.detectedCollider[i].GetComponent<enemy1>().CheckSurroundings();
            }
        }


        if (ChaceTarget)
        {
            Target = chaceZone.currentCollider.GetComponent<Transform>();
        }
        else
        {
            Target = null;
        }

    }

    private void FixedUpdate()
    {

        if (damageable.IsAlive)
        {
            //如果没破防
            if (!damageable.IsBroken)
            {
                //如果有追踪目标
                if (ChaceTarget)
                {
                    ChaceMode();
                }

                //没有追踪目标
                else
                {
                    DefaultMode();

                }

            }
            if (!touchingDirection.IsGrounded && touchingDirection.IsOnWall)
            {
                rb.velocity = Vector2.zero;

            }
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            //debug
            Debug.LogError("walk direction error");
        }
    }

    private void DefaultMode()
    {
        OnCheckPoint();

        LostTargget();

        if (CanMove)
        {
            rb.velocity = new Vector2(moveSpeed * walkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }


        if (touchingDirection.IsGrounded && touchingDirection.IsOnWall)
        {
            StartCoroutine(FlipOnce());
            
        }
    }

    IEnumerator FlipOnce()
    {
        int i = 0;
        FlipDirection();
        while (i < 1)
        {
            i++;
            if (!ChaceTarget)
            {
                rb.velocity = new Vector2(moveSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }

    }


    private void ChaceMode()
    {
        IsWaiting = false;
        waitTime = WaitTime;
        //敌人转身
        if (CanMove && damageable.IsAlive && Target != null)
        {
            if (transform.localPosition.x < Target.localPosition.x)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else
            {
                WalkDirection = WalkableDirection.Left;
            }
        }
        if (!BeingHit)
        {
            if (AttackTarget)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, chaceStopRate), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(chaceSpeed * walkDirectionVector.x, rb.velocity.y);
            }
        }
        
        //巡逻点记录
        if (Mathf.Abs(rb.transform.position.x - currentPatrol.x) <= 1f)
        {
            if (currentPatrol.x == PatrolR.x)
            {
                currentPatrol = PatrolL;
            }
            else
            {
                currentPatrol = PatrolR;
            }
        }
    }



    public void OnHit(int damage, Vector2 knockback)
    {
        if (damageable.IsHit)
        {
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
            StartCoroutine(OnHitCo());
            GetComponentInChildren<HPBar>().UpdateHp();
        }

    }

    IEnumerator OnHitCo()
    {
        float timer = 0;
        while (timer < 0.1f)
        {
            BeingHit = true;
            timer += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        BeingHit = false;
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }


    private void LostTargget()
    {
        if (lostTarget)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                CanMove = false;
                IsWaiting = true;

            }
            else
            {

                CanMove = true;
                IsWaiting = false;
                lostTarget = false;
                waitTime = WaitTime;

            }
            if (chaceZone.detectedCollider.Count > 0)
            {
                ChaceTarget = true;
            }
        }
    }

    private void OnCheckPoint()
    {
        if (Mathf.Abs(rb.transform.position.x - currentPatrol.x) <= 1f)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                CanMove = false;
                IsWaiting = true;

            }
            else
            {
                FlipDirection();
                if (currentPatrol.x == PatrolR.x)
                {
                    currentPatrol = PatrolL;
                }
                else
                {
                    currentPatrol = PatrolR;
                }

                CanMove = true;
                IsWaiting = false;
                waitTime = WaitTime;

            }
        }
    }

    public void CheckSurroundings()
    {
        timeSinceAlert = 0;
        CanMove = false;
        StartCoroutine(StartChecking());
    }

    IEnumerator StartChecking()
    {
        while (timeSinceAlert < checkTime)
        {
            timeSinceAlert += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        CanMove = true;
        if (chaceZone.detectedCollider.Count > 0 && chaceZone.currentCollider.GetComponent<Damageable>().IsAlive)
        {
            ChaceTarget = true;
            Target = chaceZone.currentCollider.GetComponent<Transform>();


        }
    }


}
