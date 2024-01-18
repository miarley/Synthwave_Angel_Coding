using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachDamge : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    private float passedTime;
    public float damageCD = 0.2f;
    private bool damageAffact = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {

            if (!damageable.IsInvincible && damageAffact)
            {

                    Vector2 deliveredknockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
                    bool gotHit = damageable.Hit(attackDamage, deliveredknockback,0,false);

                    if (gotHit)
                    {
                        Debug.Log(collision.name + "hit for" + attackDamage);
                    }
                damageAffact = false;

            }

        }
    }

    private void Update()
    {
        if (!damageAffact)
        {
            if (passedTime > damageCD)
            {
                damageAffact = true;
                passedTime = 0;
            }

            passedTime += Time.deltaTime;
        }
        if (!GetComponentInParent<Damageable>().IsAlive)
            GetComponent<Collider2D>().enabled = false;
    }
}
