using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AnticheatTextScript : MonoBehaviour
{
    void Start()
    {
        time = 5;
        t = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (time > 1)
        {
            time -= Time.deltaTime;
            t.text = $"You have been banned from Hair Basics Classic for 0 day(s), 0 hour(s), 0 minute(s) and {Mathf.CeilToInt(time)} second(s).\n\nReason: Using modifiers";
            return;
        }
        SceneManager.LoadScene("MainMenu");
    }
    TMP_Text t;
    float time;
}
