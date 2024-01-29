using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    //what the camera is tracking
    public Transform target;

    //can offset the camera if needed
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //updates camera to target pos + offset value
        transform.position = target.position + offset;
    }
}
