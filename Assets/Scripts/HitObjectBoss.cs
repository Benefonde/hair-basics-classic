using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectBoss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int rng = Random.Range(0, objects.Length - 1);
        gameObject.GetComponent<MeshFilter>().mesh = objects[rng];
    }

    public Mesh[] objects;
}
