using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGenerate : MonoBehaviour
{
    private float i = 0;
    SpriteRenderer sp;
    public Color flashColor;

    // Update is called once per frame
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetBool(AnimationStrings.isSliding))
        {
            
            i++;
            if (i==10)
            {
                ShadowPool.instance.GetFromPool();
                i = 0;
            }
            sp.material.SetColor("_FlashColor", flashColor);
            sp.material.SetFloat("_FlashAmount", (float)0.3);

        }
    }
}
