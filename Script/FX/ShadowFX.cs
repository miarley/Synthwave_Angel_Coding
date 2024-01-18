using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFX : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color Color;

    public float activeTime;
    public float activeStart;

    private float alpha;
    public float alphaSet;
    public float alphaMultiplier;
    public Color flashColor;
    private float startH, startS, startV, outputH;


    private void OnEnable()
    {
        player = GameObject.Find("player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();


        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        thisSprite.flipX = playerSprite.flipX;

        activeStart = Time.time;

        thisSprite.material.SetFloat("_FlashAmount", (float)0.7);
        thisSprite.material.SetColor("_FlashColor", flashColor);
        Color.RGBToHSV(flashColor,out startH,out startS,out startV);
        outputH = startH;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;

        outputH += 0.005f;

        if (outputH == 1)
            outputH = 0;

        Color = new Color(1, 1, 1, alpha);
        thisSprite.material.SetColor("_FlashColor", Color.HSVToRGB(outputH,startS,startV));

        thisSprite.color = Color;

        if(Time.time > activeStart + activeTime)
        {
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }


}
