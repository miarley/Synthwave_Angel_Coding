using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorFade : MonoBehaviour
{
    // Start is called before the first frame update
    public float fresnel;
    private void OnEnable()
    {
        gameObject.GetComponent<Image>().material.SetFloat("_Fresnel", -3);
        gameObject.GetComponent<Image>().material.SetFloat("_Darkness",5);
        gameObject.GetComponent<Image>().material.SetVector("_Color", new Vector4(1, 1, 1, 1));
        Time.timeScale = 0.0f;

    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
