using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    public int attackDamage = 50;
    public Vector2 knockback = Vector2.zero;
    public List<Collider2D> attackCol = new List<Collider2D>();
    public List<Collider2D> attackedCol = new List<Collider2D>();
    public bool hitOnce;
    public int maxHitCount = 1;
    private int hitCount = 0;
    public GameObject attackFX;
    public bool isPenetrating;

    public int attackBroke = 10;


    // Start is called before the first frame update
    private void Awake()
    {

    }

 
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject fu = this.transform.parent.gameObject;
        Damageable damageable = collision.GetComponent<Damageable>();
        hitOnce = false;
        hitCount = 0;

        if (damageable != null)
        {
            
            if  (!damageable.IsInvincible)
            {

                for (int i = 0; i < attackCol.Count; i++)
                {
                    if (attackCol[i] == collision)
                    {
                        attackedCol.Add(attackCol[i]);
                    }
                }
                for (int i = 0; i < attackedCol.Count; i++)
                {
                    if (attackedCol[i] == collision)
                    {
                        hitCount++;
                    }
                }
                if (hitCount >= maxHitCount)
                {
                    hitOnce = true;
                }

                if (!hitOnce)
                {
                    
                    
                    
                    Vector2 deliveredknockback = transform.parent.position.x < collision.GetComponent<Transform>().position.x ? knockback : new Vector2(-knockback.x, knockback.y);


                    bool gotHit = damageable.Hit(attackDamage, deliveredknockback,attackBroke,isPenetrating);
                    float hitRate = damageable.damageRate;

                    if (gotHit)
                    {
                        //生成特效
                        if (attackFX != null && hitCount < 1)
                        {
                            attackFX.GetComponent<HitFX>().strength = hitRate;
                            SpriteRenderer spFX = attackFX.GetComponent<SpriteRenderer>();
                            spFX.flipX = gameObject.GetComponentInParent<SpriteRenderer>().flipX;
                            Instantiate(attackFX, collision.GetComponent<Transform>().position, attackFX.transform.rotation);
                            
                        }
                        attackCol.Add(collision);
                        Debug.Log(collision.name + "hit for" + attackDamage);
                    }
                }
                    
                
            }

        }
    }


 
}
