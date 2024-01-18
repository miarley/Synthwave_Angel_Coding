using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFX : MonoBehaviour
{
    public bool needFade = true;
    public float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    Color startColor;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (needFade)
        {
            timeElapsed += Time.deltaTime;

            float newAlpha = startColor.a * (1 - Mathf.Pow(timeElapsed / fadeTime, 2));

            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            if (timeElapsed > fadeTime)
            {
                Destroy(gameObject);
            }
        }
        
        

        if (!GetComponentInParent<Damageable>().IsAlive)
        {
            Destroy(gameObject);
        }
    }
}