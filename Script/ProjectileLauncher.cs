using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    SpriteRenderer sp;
    Animator animator;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void FireProjectile()
    {
        
        Vector3 vector = sp.flipX ? new Vector3 (-2, 0, 0) :  new Vector3(2, 0, 0);
        Vector3 vector2 = animator.GetBool(AnimationStrings.fireUpon) ? new Vector3(0, 1, 0) : new Vector3(0, 0, 0);
        Instantiate(projectilePrefab, transform.position+ vector+vector2, projectilePrefab.transform.rotation);
    }
}
