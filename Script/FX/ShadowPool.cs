using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> avaliableObjects = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;
        FillPool();
    }

    public void FillPool()
    {
        for(int i=0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject game)
    {
        game.SetActive(false);

        avaliableObjects.Enqueue(game);
    }

    public GameObject GetFromPool()
    {
        if (avaliableObjects.Count == 0)
        {
            FillPool();
        }
        
        var outShadow = avaliableObjects.Dequeue();

        outShadow.SetActive(true);

        return outShadow;
    }
}
