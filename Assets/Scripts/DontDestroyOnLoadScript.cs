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
        if (trophy)
        {
            Invoke(nameof(Die), 3.5f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public bool trophy;
}
