using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseUnpause : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
        }
    }
}
