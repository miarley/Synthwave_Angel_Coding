using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    Color startColor;
    public float offsetValue;
    SpriteRenderer spriteRenderer;
    public float strength;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        transform.localEulerAngles = new Vector3(transform.localRotation.x, transform.localRotation.y, Random.Range(-offsetValue, offsetValue) +transform.localRotation.z);
        transform.localScale = new Vector3(transform.localScale.x* strength, transform.localScale.y* strength, transform.localScale.z);

    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;

        float newAlpha = startColor.a * (1 - Mathf.Pow(timeElapsed / fadeTime, 2));

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

        if (timeElapsed > fadeTime)
        {
            Destroy(gameObject);
        }
    }
}
