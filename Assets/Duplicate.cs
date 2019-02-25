using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicate : MonoBehaviour {

    public GameObject objectToDuplicate;
    public int duplicates;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < duplicates; i++)
        {
            GameObject.Instantiate(objectToDuplicate);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
