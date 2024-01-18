using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionCanvas : MonoBehaviour
{
    public GameObject askForHeal;
    public float askTime = 1;
    public GameObject reloading;
    private float timeSinceSetHeal;
    Image askingIcon;
    Image reloadingIcon;
    Color healStartColor;
    Color reloadStartColor;

    private void Awake()
    {
        askingIcon = askForHeal.GetComponent<Image>();
        healStartColor = askingIcon.color;
        askingIcon.color = new Color(healStartColor.r, healStartColor.g, healStartColor.b, 0);

        reloadingIcon = reloading.GetComponent<Image>();
        reloadStartColor = reloadingIcon.color;
        reloadingIcon.color = new Color(reloadStartColor.r, reloadStartColor.g, reloadStartColor.b, 0);
    }

    public void updateAskingHeal()
    {
        askingIcon.color = healStartColor;
        timeSinceSetHeal = 0;
        askForHeal.GetComponent<Animator>().Play("default");
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        while (timeSinceSetHeal < askTime)
        {
            float newAlpha = healStartColor.a * (Mathf.Sqrt(1 - timeSinceSetHeal / askTime));
            askingIcon.color = new Color(healStartColor.r, healStartColor.g, healStartColor.b, newAlpha);

            timeSinceSetHeal += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
 

        }
        askingIcon.color = new Color(healStartColor.r, healStartColor.g, healStartColor.b, 0);
        askForHeal.GetComponent<Animator>().Rebind();
    }

    public void OnUpdateReloading(bool isReloading )
    {
        float newAlpha;
        if (isReloading)
            newAlpha = 1;
        else
            newAlpha = 0;
        
        reloadingIcon.color = new Color(reloadStartColor.r, reloadStartColor.g, reloadStartColor.b, newAlpha);
        reloading.GetComponent<Animator>().Play("default");
    }



}
