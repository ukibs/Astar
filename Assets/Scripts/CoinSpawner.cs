﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    //
    public float timeBetweenSpawns = 2;
    public GameObject coinPrefab;
    public int maxActiveCoins = 20;
    //
    //private Ingredient[] presenetIngredients;
    private float timeFromLastSpawn;
    //GameObject[] ingredientPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        //
        //Debug.Log("Spawner en marcha");
        //
        float dt = Time.deltaTime;
        //
        timeFromLastSpawn += dt;
        //
        if(timeFromLastSpawn >= timeBetweenSpawns)
        {
            SpawnCoin();
            timeFromLastSpawn -= timeBetweenSpawns;
        }
    }

    void SpawnCoin()
    {
        //
        Coin[] activeCoins = FindObjectsOfType<Coin>();
        if(activeCoins.Length < maxActiveCoins)
        {
            //
            float xToUse = UnityEngine.Random.Range(-10, 10);
            float zToUse = UnityEngine.Random.Range(-10, 10);
            RaycastHit hitInfo;
            if(Physics.Raycast(new Vector3(xToUse, 10, zToUse), -Vector3.up, out hitInfo, 15))
            {
                if(hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Unwalkable"))
                {
                    Instantiate(coinPrefab, new Vector3(xToUse, 1, zToUse), coinPrefab.transform.rotation);
                    //Debug.Log("Spawning coin ");
                }
                else
                {
                    //Debug.Log("Pega en duro ");
                }
            }
            
        }
        else
        {
            //Debug.Log("Max coins reached");
        }

    }
    
}
