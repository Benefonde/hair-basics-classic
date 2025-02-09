using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionScript : MonoBehaviour
{
    private void Start()
    {
        txt = GetComponent<TMP_Text>();
    }

    public void SetText(string a)
    {
        txt.text = a;
    }
    public void SetPlayerPref(string a)
    {
        playerPref = a;
    }

    public void SetTextScore(string a)
    {
        txt.text = a + $"\nScore: {PlayerPrefs.GetInt(playerPref)}";
    }

    TMP_Text txt;
    string playerPref;
}
