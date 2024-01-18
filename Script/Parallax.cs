using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;
    public Vector2 startPosition;
    float startZ;

    Vector2 Travel => (Vector2)cam.transform.position - startPosition;


    public float parallaxFactor;


    private void Awake()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector2 newPos = startPosition + Travel* parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }


}
