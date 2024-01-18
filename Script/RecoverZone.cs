using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecoverZone : MonoBehaviour
{
    public UnityEvent noColliderRemain;

    public List<Collider2D> detectedCollider = new List<Collider2D>();
    private Collider2D col;
    public Collider2D currentCollider;
    public float healTime=2f;
    public float moveSpeed = 7f;
    public int restoreAmount = 66;
    private float timeSinceInvoke;
    public int attackDamage = 30;
    public Vector2 knockback = Vector2.zero;
    public bool isPenetrating;
    public int attackBroke = 0;
    public float breathTime;
    SpriteRenderer sp;
    public SpriteRenderer sp2;
    public SpriteRenderer sp3;
    public SpriteRenderer sp4;
    Color startColor;
    public float revealTime = 0.5f;

    // Start is called before the first frame update
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        timeSinceInvoke = 0;
        sp = GetComponent<SpriteRenderer>();
        startColor = sp.color;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider.Add(collision);
        currentCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider.Remove(collision);
        currentCollider = null;
    }

    private void Update()
    {
        timeSinceInvoke += Time.deltaTime;

        if (timeSinceInvoke < revealTime)
        {
            float newAlpha = startColor.a * (timeSinceInvoke / revealTime);
            sp.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        }

        if(timeSinceInvoke + 0.2f > healTime)
        {
            //StartCoroutine(Flashing());
            sp4.enabled = true;
            sp4.GetComponent<Animator>().Play("default");
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("player").transform.position, moveSpeed * Time.deltaTime);

        if (timeSinceInvoke > healTime)
        {
            for(int i = 0; i < detectedCollider.Count; i++)
            {
                GameObject gameObject = detectedCollider[i].gameObject;

                if (gameObject.CompareTag("Player"))
                {
                    detectedCollider[i].GetComponent<PlayerController>().OnHeal(restoreAmount);
                }
                if(gameObject.CompareTag("Enemy"))
                {
                    Vector2 deliveredknockback = transform.position.x < gameObject.GetComponent<Transform>().position.x ? knockback : new Vector2(-knockback.x, knockback.y);
                    detectedCollider[i].GetComponent<Damageable>().Hit(attackDamage, deliveredknockback, attackBroke, isPenetrating);

                }
                
               
            }
            col.enabled = false;
            sp.enabled = false;
            sp2.enabled = false;
            sp3.enabled = false;

            if (sp4.GetComponent<Animator>().GetBool(AnimationStrings.isDone))
            {
                Destroy(gameObject);
            }

            

        }


    }

    //IEnumerator Flashing()
    //{
        
    //    while (timeSinceInvoke < healTime)
    //    {
    //        sp.material.SetColor("_FlashColor", new Color(1, 1, 1));
    //        sp.material.SetFloat("_FlashAmount", (Mathf.Cos(Time.time*50)+1)/2);
    //        yield return new WaitForSecondsRealtime(0.05f);
    //    }


    //}

}

