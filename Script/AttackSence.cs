using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSence : MonoBehaviour
{
    private static AttackSence instance;
    private bool isShake;
    private bool isPause;

    public static AttackSence Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AttackSence>();
            return instance;
        }
    }

    public void HitPause(int duration)
    {
        if (!isPause)
        {
            StartCoroutine(Pause(duration));
        }
            
    }
    
    IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        isPause = true;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
        isPause = false;
    }



    public void CameraShake(float duration, float strength)
    {
        if (!isShake)
            StartCoroutine(Shake(duration, strength));
    }

    IEnumerator Shake(float duration, float strength)
    {
        isShake = true;
        Transform camera = Camera.main.transform;
        CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        Vector3 startPosition = camera.position;

        while (duration > 0)
        {
            cinemachineBrain.enabled=false;
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        cinemachineBrain.enabled = true;
        camera.position = startPosition;
        isShake = false;
    }
}
