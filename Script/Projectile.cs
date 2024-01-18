using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 30;
    public Vector2 knockback = Vector2.zero;
    public float moveSpeed = 20f;
    Rigidbody2D rb;
    private float lifeTimer;
    [SerializeField]
    private float maxTime=2f;
    private float playerDirection;
    Vector3 targetPosition;
    private float randomValue2;
    public GameObject attackFX;
    SpriteRenderer sp;
    public bool isPenetrating;
    Vector3 initialPosition;

    public int brock = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        playerDirection = GameObject.Find("player").GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        targetPosition = GameObject.Find("player").GetComponent<PlayerController>().shootingTarget;
        float angle = GameObject.Find("player").GetComponent<PlayerController>().shootingAngle;
        sp.flipX = playerDirection > 0 ? false : true;
        transform.localEulerAngles = new Vector3(transform.localRotation.x, transform.localRotation.y,angle*playerDirection);
        randomValue2 = Random.Range(-1, 1);
        sp.material.SetColor("_FlashColor",new Color(1,0.8f,0.08f,1));
        sp.material.SetFloat("_FlashAmount", (float)0.8);
        initialPosition= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPosition != Vector3.zero)
        {
            Vector3 angle = targetPosition - initialPosition;
            float chu = angle.x + angle.y;
            rb.velocity = new Vector2(moveSpeed * angle.x / chu* playerDirection, moveSpeed * angle.y / chu* playerDirection);

        }
        else
            rb.velocity = new Vector2(moveSpeed * playerDirection,randomValue2 );

        lifeTimer += Time.deltaTime;
        if (lifeTimer > maxTime)
        {
            lifeTimer = 0;
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        Vector2 deliveredknockback = rb.transform.localScale.x> 0 ? knockback : new Vector2(-knockback.x, knockback.y);

        bool gotHit = damageable.Hit(damage, deliveredknockback,brock, isPenetrating);
        float hitRate = damageable.damageRate;

        if (gotHit)
        {
            if (attackFX != null)
            {
                attackFX.GetComponent<HitFX>().strength = hitRate;
                SpriteRenderer spFX = attackFX.GetComponent<SpriteRenderer>();
                spFX.flipX = sp.flipX;
                Instantiate(attackFX, transform.position, attackFX.transform.rotation);
            }
            Debug.Log(collision.name + "hit for" + damage);
            Destroy(gameObject);
        }

    }
}
