using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    private void Start()
    {
        disstance = Vector3.Distance(panino.position, transform.position) / 2;
        transform.localScale = new Vector3(disstance, disstance, disstance);
    }

    void Update()
    {
        disstance = Vector3.Distance(panino.position, transform.position) / 2;
        transform.localScale = new Vector3(disstance, disstance, disstance);
    }

    public Transform panino;

    float disstance;
}
