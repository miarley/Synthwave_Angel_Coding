using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BlurFade : MonoBehaviour

    
{

    public float fresnel;
    private float nowFresnel;
    private void OnEnable()
    {
        gameObject.GetComponent<Image>().material.SetFloat("_Fresnel",0);
        nowFresnel = gameObject.GetComponent<Image>().material.GetFloat("_Fresnel");
        gameObject.GetComponent<Image>().material.SetFloat("_Darkness", 1);
        StartCoroutine(UpdateFade());
    }

    IEnumerator UpdateFade()
    {
        gameObject.GetComponent<Image>().material.SetVector("_Color", new Vector4(1, 1, 1, 1));
        while (fresnel < nowFresnel)
        {
            nowFresnel -= 0.01f;
            gameObject.GetComponent<Image>().material.SetFloat("_Fresnel", nowFresnel);
            gameObject.GetComponent<Image>().material.SetFloat("_Darkness", nowFresnel*(-5)+3);

            yield return new WaitForSecondsRealtime(0.005f);
        }
        nowFresnel = 0;
    }


}
