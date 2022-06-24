using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField] Vector2[] launcherPositions;
    [SerializeField] Vector3[] launcherRotations;

    int lookingDirection = 0;
    float distanceFromPlayer = 2.5f;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow)){
            if(Input.GetKey(KeyCode.UpArrow)){
                //Debug.Log("upright");
                lookingDirection = 1;
            }
            else if(Input.GetKey(KeyCode.DownArrow)){
                //Debug.Log("downright");
                lookingDirection = 7;
            }
            else{
                //Debug.Log("right");
                lookingDirection = 0;
            }
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            if(Input.GetKey(KeyCode.UpArrow)){
                //Debug.Log("upleft");
                lookingDirection = 3;
            }
            else if(Input.GetKey(KeyCode.DownArrow)){
                //Debug.Log("downleft");
                lookingDirection = 5;
            }
            else{
                //Debug.Log("left");
                lookingDirection = 4;
            }
        }
        else if(Input.GetKey(KeyCode.UpArrow)){
            //Debug.Log("up");
            lookingDirection = 2;
        }
        else if(Input.GetKey(KeyCode.DownArrow)){
            //Debug.Log("down");
            lookingDirection = 6;
        }

        MakeRotate();
    }

    void MakeRotate()
    {
        transform.position = playerTransform.position + new Vector3(distanceFromPlayer,0,0);
        transform.rotation = Quaternion.Euler(0,0,0);
        transform.RotateAround(playerTransform.position, new Vector3(0,0,1), lookingDirection*45);
    }
}