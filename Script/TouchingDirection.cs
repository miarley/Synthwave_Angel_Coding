using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    public int jiechu;

    RaycastHit2D[] groundHits = new RaycastHit2D[1];
    RaycastHit2D[] wallHits = new RaycastHit2D[1];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[1];

    Rigidbody2D rb;
    CapsuleCollider2D touchingCol;
    Animator Animator;

    [SerializeField]
    private bool _IsGrounded;
  
    public bool IsGrounded { get 
        {
            return _IsGrounded;
        } private set 
        {
            if(_IsGrounded != value)
            {
                IsOnWall = false;
            }
            _IsGrounded = value;
            Animator.SetBool(AnimationStrings.isGrounding, value);
        }
    }

    [SerializeField]
    private bool _IsOnWall= false;
    public Vector2 WallCheckDirection => !gameObject.GetComponent<SpriteRenderer>().flipX ? Vector2.right : Vector2.left;

    public bool IsOnWall
    {
        get
        {
            return _IsOnWall;
        }
        private set
        {
            _IsOnWall = value;
            Animator.SetBool(AnimationStrings.isOnWall, value);

        }
    }

    [SerializeField]
    private bool _IsOnCeiling= false;

    public bool IsOnCeiling
    {
        get
        {
            return _IsOnCeiling;
        }
        private set
        {
            _IsOnCeiling = value;
            Animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }



    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<CapsuleCollider2D>();
        Animator = GetComponent<Animator>();
    }


      private void FixedUpdate()
    {
       
        IsOnWall = touchingCol.Cast(WallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        jiechu = touchingCol.Cast(WallCheckDirection, castFilter, wallHits, wallDistance);
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;




    }

}
