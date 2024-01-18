using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokeBar : MonoBehaviour
{
    public Image broke;
    public Image[] alpha;
    Damageable Damageable;
    Color[] startColor = new Color[10];

    private float brokeValue;
    private float maxBroke;
    private float timeElapsed;

    public float fadeTime = 2f;
    public float increaseSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        Damageable = GetComponentInParent<Damageable>();
        brokeValue = 0;
        maxBroke = Damageable.MaxDefence;
        alpha = GetComponentsInChildren<Image>();

        for (int i = 0; i < alpha.Length; i++)
        {
            startColor[i] = alpha[i].color;
            alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, 0);
        }

    }

    // Update is called once per frame
    private void Update()
    {

        if (alpha[0].color.a > 0 && timeElapsed < fadeTime)
        {
            timeElapsed += Time.deltaTime;

            float alphaChange = 1 - Mathf.Pow(timeElapsed / fadeTime, 4);

            if (Damageable.IsBroken)
            {
                alphaChange = 0;
            }

            for (int i = 0; i < alpha.Length; i++)
            {
                float newAlpha = startColor[i].a * alphaChange * broke.fillAmount;
                alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, newAlpha);
            }
        }



        float currentValue = Damageable.currentBroke / maxBroke;

        if(currentValue < broke.fillAmount)
        {
            broke.fillAmount -= 0.001f;
        }

        if (!Damageable.IsAlive)
        {
            Destroy(this.gameObject);
        }

        if (Damageable.IsBroken)
        {
            broke.fillAmount = 0;
        }
    }

    public void UpdateBroke()
    {
        StartCoroutine(UpdateBrokeCo());
    }

    IEnumerator UpdateBrokeCo()
    {
        for (int i = 0; i < alpha.Length; i++)
        {
            alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, startColor[i].a);
        }
        
        timeElapsed = 0;

        brokeValue = Damageable.currentBroke;

        float currentValue = brokeValue / maxBroke;
        if (currentValue > 1)
        {
            currentValue = 0;
        }
        while (broke.fillAmount < currentValue)
        {
            broke.fillAmount += increaseSpeed;
            yield return new WaitForSeconds(0.005f);
        } 
        if(broke.fillAmount > currentValue)
        {
            broke.fillAmount = currentValue;
        }
            
    }


}
