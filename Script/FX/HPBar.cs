using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public Image hp;
    public Image hpeffect;
    public Image[] alpha;
    Damageable Damageable;
    Color[] startColor = new Color[10];


    private float Health;
    private float MaxHealth;

    public float hurtSpeed = 0.005f;
    private float timeElapsed;

    public float fadeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Damageable=GetComponentInParent<Damageable>();
        MaxHealth = Damageable.MaxHealth;
        Health = MaxHealth;
        alpha = GetComponentsInChildren<Image>();
        
        for (int i = 0; i < alpha.Length; i++)
        {
            startColor[i] = alpha[i].color;
            alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, 0);
        }

    }

    private void Update()
    {

        if (alpha[0].color.a > 0 && timeElapsed < fadeTime)
        {
            timeElapsed += Time.deltaTime;

            float alphaChange = 1 - Mathf.Pow(timeElapsed / fadeTime, 4);
            for (int i = 0; i < alpha.Length; i++)
            {
                float newAlpha = startColor[i].a * alphaChange;
                alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, newAlpha);
            }
        }

        if (!Damageable.IsAlive)
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdateHp()
    {
        StartCoroutine(UpdateHpCo());
    }

    IEnumerator UpdateHpCo()
    {

        for (int i = 0; i < alpha.Length; i++)
        {
            alpha[i].color = new Color(startColor[i].r, startColor[i].g, startColor[i].b, startColor[i].a);
        }
        Health = Damageable.Health;

        timeElapsed = 0;

        hp.fillAmount = Health / MaxHealth+0.05f;

        while (hpeffect.fillAmount > hp.fillAmount)
        {
            hpeffect.fillAmount -= hurtSpeed;
            yield return new WaitForSeconds(0.005f);
        }
        if(hpeffect.fillAmount<hp.fillAmount)
            hpeffect.fillAmount = hp.fillAmount;
    }




}
