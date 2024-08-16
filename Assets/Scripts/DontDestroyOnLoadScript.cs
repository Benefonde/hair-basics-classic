using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (die)
        {
            Invoke(nameof(Die), dieTime);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public bool die;

    public float dieTime = 3.5f;
}
