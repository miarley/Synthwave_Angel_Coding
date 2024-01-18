using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noColliderRemain;
    
    public List<Collider2D> detectedCollider = new List<Collider2D>();
    private Collider2D col;
    public Collider2D currentCollider;


    // Start is called before the first frame update
    private void Awake()
    {
        col = GetComponent<Collider2D>();
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

        if (detectedCollider.Count <= 0)
        {
            noColliderRemain.Invoke();
        }
    }
}


