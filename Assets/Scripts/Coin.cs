using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //
    public float distanceToGetCoin = 5;
    //
    private Transform player;
    private HUD hud;
    // Start is called before the first frame update
    void Start()
    {
        //Cutre
        player = GameObject.Find("Chef1").transform;
        hud = FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        if((player.position - transform.position).magnitude < distanceToGetCoin)
        {
            // Efecto de coger monead
            //Debug.Log("Moneda cogida");
            hud.AddCoin();
            //
            Destroy(gameObject);
        }
    }
}
