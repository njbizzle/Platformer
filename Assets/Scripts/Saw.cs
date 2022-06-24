using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.rigidbody == player.PlayerRigidbody2D){
            player.PlayerRespawn();
        }
    }
}
