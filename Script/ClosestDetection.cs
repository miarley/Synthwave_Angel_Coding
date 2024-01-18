using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestDetection : MonoBehaviour
{
    [SerializeField]
    List<Transform> targets = new List<Transform>();
    public Transform closestTarget;
    List<Damageable> damageable = new List<Damageable>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targets.Add( collision.GetComponent<Transform>());
        damageable.Add(collision.GetComponent<Damageable>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision.GetComponent<Transform>());
        damageable.Remove(collision.GetComponent<Damageable>());
    }
    public void OnShoot()
    {
        closestTarget = null;
        if (targets.Count > 0)
        {
            float mindis = Vector3.Distance(transform.position, targets[0].position);
            if (damageable[0].IsAlive)
            {
                closestTarget = targets[0];
            }

                for (int i = 1; i < targets.Count; i++)
                {
                    float nextdis = Vector3.Distance(transform.position, targets[i].position);
                if (nextdis < mindis && damageable[i].IsAlive)
                    {
                        mindis = nextdis;
                        closestTarget = targets[i];
                    }
                }

        }
        
            
    }

}
