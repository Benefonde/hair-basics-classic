using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsSet : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("fps", 60);
        if (PlayerPrefs.GetInt("vsync") == 1)
        {
            QualitySettings.vSyncCount = 1;
        }
    }
}
