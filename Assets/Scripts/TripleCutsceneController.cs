using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleCutsceneController : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("pActive", 1) == 1)
        {
            characters[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("mActive", 1) == 1)
        {
            characters[1].SetActive(true);
        }
        if (PlayerPrefs.GetInt("aActive", 1) == 1)
        {
            characters[2].SetActive(true);
        }
        if (PlayerPrefs.GetInt("yActive", 0) == 1)
        {
            characters[3].SetActive(true);
        }
    }

    public GameObject[] characters; //0 panino 1 miko 2 alger 3 yellow face
}
