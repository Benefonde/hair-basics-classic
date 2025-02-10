using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamePos : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = otherObj.position;
        transform.rotation = otherObj.rotation;
    }

    public Transform otherObj;
}
