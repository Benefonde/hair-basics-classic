using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamePos : MonoBehaviour
{
    private void Start()
    {
        if (retroCam)
        {
            myCam = GetComponent<Camera>();
            mainCam = Camera.main;
        }    
    }

    void Update()
    {
        if (retroCam)
        {
            myCam.fieldOfView = mainCam.fieldOfView;
        }
    }

    private void FixedUpdate()
    {
        transform.position = otherObj.position;
        transform.rotation = otherObj.rotation;
    }

    public Transform otherObj;
    Camera mainCam;
    Camera myCam;
    public bool retroCam;
}
