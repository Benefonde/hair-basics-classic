using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorBaldiScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string a = PlayerPrefs.GetString("CurrentMode");
        if (a == "speedy" || a == "miko" || a == "triple" || a == "stealthy" || a == "alger")
        {
            return;
        }
        GetComponent<AudioSource>().PlayOneShot(hi);
        FindObjectOfType<SubtitleManager>().Add3DSubtitle("Hi. Welcome to my ball.", hi.length, Color.cyan, transform);
    }

    public AudioClip hi;
}
