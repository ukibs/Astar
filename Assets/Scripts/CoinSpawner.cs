using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    //
    public float timeBetweenSpawns = 2;
    public GameObject coinPrefab;
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
        Debug.Log("Spawner en marcha");
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
        
        float xToUse = UnityEngine.Random.Range(-10, 10);
        float zToUse = UnityEngine.Random.Range(-10, 10);
        Instantiate(coinPrefab, new Vector3(xToUse, 1, zToUse), Quaternion.identity);
        Debug.Log("Spawning coin ");

    }
    
}
