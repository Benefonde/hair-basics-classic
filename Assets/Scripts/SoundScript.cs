using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    private void Start()
    {
        if (StaySize)
        {
            disstance = Vector3.Distance(panino.position, transform.position) * scale;
            transform.localScale = new Vector3(disstance, disstance, disstance);
        }
    }

    void Update()
    {
        if (StaySize)
        {
            disstance = Vector3.Distance(panino.position, transform.position) * scale;
            transform.localScale = new Vector3(disstance, disstance, disstance);
        }
    }

    public void GoHere(Vector3 pos)
    {
        transform.position = pos;
    }

    public Transform panino;

    float disstance;

    public float scale;

    public bool StaySize;
}