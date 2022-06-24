using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] Player player;
    Vector2 posRight;
    Vector2 posLeft;
    Vector2 difference;
    bool targetFacingRight = true;

    void Start()
    {
        player = FindObjectOfType<Player>();

        posRight = transform.position;
        posLeft = new Vector2(-transform.position.x, transform.position.y);
    }

    void Update()
    {
        if(player.facingRight && !targetFacingRight){
            transform.position = new Vector2(transform.position.x + Mathf.Abs(transform.position.x - player.transform.position.x) * 2, transform.position.y);
            targetFacingRight = true;
        }
        else if(!player.facingRight && targetFacingRight){
            transform.position = new Vector2(transform.position.x + Mathf.Abs(transform.position.x - player.transform.position.x) * -2, transform.position.y);
            targetFacingRight = false;
        }
        Debug.Log(targetFacingRight.ToString() + player.facingRight.ToString());
    }
}
