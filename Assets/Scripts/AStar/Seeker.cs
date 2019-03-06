using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seeker : MonoBehaviour {

    private NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        //
        bool mouseLeft = Input.GetMouseButton(0);
        if (mouseLeft)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1000))
            {
                navMeshAgent.SetDestination(hitInfo.point);
            }
        }
	}
}
