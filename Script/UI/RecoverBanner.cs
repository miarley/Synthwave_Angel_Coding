using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverBanner : MonoBehaviour
{
    private int recoverChance;
    public GameObject recoverIcon;
    public GameObject canvasObject;
    [SerializeField]
    private GameObject[] recoverCount = new GameObject[10];
    
    // Start is called before the first frame update
    void Start()
    {
        recoverChance = GameObject.Find("player").GetComponent<PlayerController>().HealCount;
        Debug.Log(recoverChance);

        for (int i = 0; i < recoverChance; i++)
        {
            recoverCount[i] = Instantiate(recoverIcon, new Vector3((i * 65) - 570, 450, 0), 
                recoverIcon.transform.rotation);
            recoverCount[i].transform.SetParent(canvasObject.transform, false);
            recoverCount[i].transform.localScale = new Vector3(1,1,1);
        }
    }


    public void UpdateRecover(int count)
    {
        StartCoroutine(UpdateRecovrCo(count));
    }

    IEnumerator UpdateRecovrCo(int currentCount)
    {
        while (recoverChance > currentCount)
        {
            recoverCount[recoverChance-1].GetComponent<Animator>().SetBool(AnimationStrings.used, true);
            recoverChance--;
            yield return new WaitForSeconds(0.005f);
        }
    }

}
