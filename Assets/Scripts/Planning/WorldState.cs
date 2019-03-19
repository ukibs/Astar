using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public Vector3 cPos;
    public World.WorldStateMask mask;
    public List<Ingredient> ingredientsKept = new List<Ingredient>();

    public WorldState()
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
