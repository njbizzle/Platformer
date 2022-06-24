using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunParticles : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if(player.playerVel != Vector2.zero){
            
        }
    }
}
