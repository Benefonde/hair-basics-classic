using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyChalkEraserScript : MonoBehaviour
{
    void Start()
    {
        base.StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float t = 61;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        GetComponent<BoxCollider>().enabled = false;
        Destroy(base.gameObject);
    }

    public ParticleSystem ps;
}