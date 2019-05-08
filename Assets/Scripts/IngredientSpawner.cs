using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    //
    public float timeBetweenSpawns = 2;
    //
    private Ingredient[] presenetIngredients;
    private float timeFromLastSpawn;
    GameObject[] ingredientPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        //
        ingredientPrefabs = Resources.LoadAll<GameObject>("Prefabs/Ingredients");
        Debug.Log(ingredientPrefabs);
        //
        presenetIngredients = FindObjectsOfType<Ingredient>();
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
            SpawnIngredient();
            timeFromLastSpawn -= timeBetweenSpawns;
        }
    }

    void SpawnIngredient()
    {
        //
        presenetIngredients = FindObjectsOfType<Ingredient>();
        //
        int indexToSpawn = UnityEngine.Random.Range(0, ingredientPrefabs.Length);
        //
        if (!IsPresent(indexToSpawn))
        {
            float xToUse = UnityEngine.Random.RandomRange(-10, 10);
            float zToUse = UnityEngine.Random.RandomRange(-10, 10);
            Instantiate(ingredientPrefabs[indexToSpawn], new Vector3(xToUse, 1, zToUse), Quaternion.identity);
            //Debug.Log("Spawning " + ingredientPrefabs[indexToSpawn]);
        }
        else
        {
            //Debug.Log(ingredientPrefabs[indexToSpawn] + " already present");
        }
            
    }

    bool IsPresent(int index)
    {
        Ingredient ingredientToSpawn = ingredientPrefabs[index].GetComponent<Ingredient>();

        for(int i = 0; i < presenetIngredients.Length; i++)
        {
            if (ingredientToSpawn.type == presenetIngredients[i].type)
                return true;
        }

        return false;
    }
}
