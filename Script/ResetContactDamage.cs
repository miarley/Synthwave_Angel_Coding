using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetContactDamage : MonoBehaviour
{
    Attack attack;
    public float validCD = 0.3f;
       [SerializeField]
    private float passedTime;




    private void Awake()
    {  
        attack = GetComponentInChildren<Attack>();
    }
    // Update is called once per frame
    void Update()
    {
        if (attack.attackCol.Count > 0)
        {
            if (passedTime > validCD)
            {
                attack.attackCol.Clear();
                Debug.Log("ffgggff");
                attack.attackedCol.Clear();
                passedTime = 0;
            }

            passedTime += Time.deltaTime;
        }
        else
        {
            passedTime = 0;
            attack.hitOnce = false;
        }
        
    }
}
