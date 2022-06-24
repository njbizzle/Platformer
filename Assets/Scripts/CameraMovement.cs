using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float cameraSpeed;
    
    [SerializeField] bool freeCamera;
    [SerializeField] bool snap;

    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] float yMin;
    [SerializeField] float yMax;
    
    Vector3 currentPos;
    Vector3 targetPos;
    float cameraZ;

    void Start()
    {
        cameraZ = transform.position.z;
    }

    void Update()
    {
        currentPos = transform.position;
        targetPos = target.position;
        Vector3 move;
        if(snap){
            move = targetPos;
        }
        else{
            move = Vector3.Lerp(currentPos, targetPos, Time.deltaTime*cameraSpeed);
        }
        if(freeCamera){
            transform.position = new Vector3 (move.x, move.y, cameraZ);
        }
        else{
            transform.position = new Vector3(
            Mathf.Clamp(move.x, xMin, xMax),
            Mathf.Clamp(move.y, yMin, yMax),
            cameraZ);
        }
    }
}
