using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = (panino.transform.position - transform.position) * 10;
    }

    void Update()
    {
        transform.localScale = (panino.transform.position - transform.position) * 10;
    }

    public Transform panino;
}
