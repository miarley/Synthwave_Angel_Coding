using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeaFade : MonoBehaviour
{
    public float fresnel;
    private float nowFresnel;
    private void OnEnable()
    {
        gameObject.GetComponent<Image>().material.SetFloat("_Fresnel", 0);
        nowFresnel = gameObject.GetComponent<Image>().material.GetFloat("_Fresnel");
        gameObject.GetComponent<Image>().material.SetFloat("_Darkness", 1);
        gameObject.GetComponent<Image>().material.SetVector("_Color", new Vector4(1, 1, 1,1));
        StartCoroutine(UpdateFade());
    }

    IEnumerator UpdateFade()
    {
        float t;
        gameObject.GetComponent<Image>().material.SetVector("_Color", new Vector4(1, 1, 1, 1));
        while (fresnel < nowFresnel)
        {
            nowFresnel -= 0.03f;
            gameObject.GetComponent<Image>().material.SetFloat("_Fresnel", nowFresnel);
            gameObject.GetComponent<Image>().material.SetFloat("_Darkness", nowFresnel * (-3) + 1);
            t = 1-nowFresnel;
            gameObject.GetComponent<Image>().material.SetVector("_Color", new Vector4(t/2,1,1,1));
            yield return new WaitForSecondsRealtime(0.005f);
        }
        nowFresnel = 0;
    }
}
